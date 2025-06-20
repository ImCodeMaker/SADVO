using AutoMapper;
using SADVO.Core.Application.Dtos.Elecciones;
using SADVO.Core.Application.Dtos.ResumenElectoral;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class EleccionesServices : GenericService<CrearEleccionDTO, UpdateEleccionDTO, EleccionDTO, Elecciones>, IEleccionesServices
	{
		private readonly IEleccionesRepository _eleccionesRepository;
		private readonly IPuestosElectivosRepository _puestosRepository;
		private readonly IPartidosPoliticosRepository _partidosRepository;
		private readonly IAsignacionCandidatosRepository _asignacionCandidatosRepository;

		public EleccionesServices(
			IEleccionesRepository eleccionesRepository,
			IPuestosElectivosRepository puestosRepository,
			IPartidosPoliticosRepository partidosRepository,
			IAsignacionCandidatosRepository asignacionCandidatosRepository,
			IMapper mapper) : base(eleccionesRepository, mapper)
		{
			_eleccionesRepository = eleccionesRepository;
			_puestosRepository = puestosRepository;
			_partidosRepository = partidosRepository;
			_asignacionCandidatosRepository = asignacionCandidatosRepository;
		}

		public async Task<EleccionDTO?> GetEleccionActivaAsync()
		{
			var eleccion = await _eleccionesRepository.GetEleccionActivaAsync();
			return eleccion != null ? _mapper.Map<EleccionDTO>(eleccion) : null;
		}

		public async Task<List<EleccionDTO>> GetAllEleccionesOrderedAsync()
		{
			var elecciones = await _eleccionesRepository.GetAllEleccionesOrderedAsync();
			var eleccionesDto = _mapper.Map<List<EleccionDTO>>(elecciones);

			// Agregar datos adicionales para cada elección
			foreach (var eleccion in eleccionesDto)
			{
				if (eleccion.Estado && eleccion.FechaFinalizacion == default(DateTime))
				{
					eleccion.EsActiva = true;
				}

				// Obtener cantidad de partidos y puestos si la elección tiene votos
				var partidosCount = await _eleccionesRepository.GetPartidosCountByEleccionAsync(eleccion.Id);
				var puestosCount = await _eleccionesRepository.GetPuestosCountByEleccionAsync(eleccion.Id);

				eleccion.CantidadPartidos = partidosCount.Count;
				eleccion.CantidadPuestos = puestosCount.Count;
			}

			return eleccionesDto;
		}

		public async Task<bool> FinalizarEleccionAsync(int eleccionId)
		{
			return await _eleccionesRepository.FinalizarEleccionAsync(eleccionId);
		}

		public async Task<(bool isValid, List<string> errors)> ValidarCreacionEleccionAsync()
		{
			var errors = new List<string>();

			// Verificar que hay al menos un puesto electivo activo
			var puestosActivos = await _puestosRepository.GetAllList();
			var puestosActivosFiltrados = puestosActivos.Where(p => p.Estado).ToList();

			if (!puestosActivosFiltrados.Any())
			{
				errors.Add("No hay puestos electivos activos para realizar una elección.");
				return (false, errors);
			}

			// Verificar que hay al menos dos partidos políticos activos
			var partidosActivos = await _partidosRepository.GetAllList();
			var partidosActivosFiltrados = partidosActivos.Where(p => p.Estado).ToList();

			if (partidosActivosFiltrados.Count < 2)
			{
				errors.Add("No hay suficientes partidos políticos para realizar una elección.");
				return (false, errors);
			}

			// Verificar que cada partido tiene candidatos para todos los puestos
			var asignaciones = await _asignacionCandidatosRepository.GetAllList();
			var asignacionesActivas = asignaciones.Where(a => a.Estado).ToList();

			foreach (var partido in partidosActivosFiltrados)
			{
				var puestosSinCandidato = new List<string>();

				foreach (var puesto in puestosActivosFiltrados)
				{
					var tieneCandidato = asignacionesActivas.Any(a =>
						a.PartidoPoliticoId == partido.Id &&
						a.PuestoElectivoId == puesto.Id);

					if (!tieneCandidato)
					{
						puestosSinCandidato.Add(puesto.Nombre);
					}
				}

				if (puestosSinCandidato.Any())
				{
					errors.Add($"El partido político {partido.Nombre} ({partido.Siglas}) no tiene candidatos registrados para los siguientes puestos electivos: {string.Join(", ", puestosSinCandidato)}.");
				}
			}

			return (!errors.Any(), errors);
		}

		public async Task<List<ResultadoEleccionDTO>> GetResultadosEleccionAsync(int eleccionId)
		{
			var resultados = await _eleccionesRepository.GetResultadosEleccionAsync(eleccionId);
			return _mapper.Map<List<ResultadoEleccionDTO>>(resultados);
		}

		public async Task<bool> CreateEleccionWithValidationAsync(CrearEleccionDTO dto)
		{
			var (isValid, errors) = await ValidarCreacionEleccionConAnioAsync(dto);
			if (!isValid)
			{
				return false;
			}

			var entity = _mapper.Map<Elecciones>(dto);
			entity.FechaCreacion = DateTime.Now;
			await _eleccionesRepository.AddAsync(entity);
			return true;
		}

		// NUEVOS MÉTODOS PARA RESUMEN ELECTORAL

		public async Task<AniosDisponiblesDTO> GetAniosDisponiblesAsync()
		{
			var anios = await _eleccionesRepository.GetAniosDisponiblesAsync();

			return new AniosDisponiblesDTO
			{
				Años = anios,
				AñoMasReciente = anios.FirstOrDefault()
			};
		}

		public async Task<List<ResumenElectoralDTO>> GetResumenElectoralPorAnioAsync(int anio)
		{
			var elecciones = await _eleccionesRepository.GetEleccionesPorAnioAsync(anio);
			var resumenList = new List<ResumenElectoralDTO>();

			foreach (var eleccion in elecciones)
			{
				var cantidadPartidos = await _eleccionesRepository.GetCantidadPartidosPorEleccionAsync(eleccion.Id);
				var cantidadCandidatos = await _eleccionesRepository.GetCantidadCandidatosPorEleccionAsync(eleccion.Id);
				var totalVotos = await _eleccionesRepository.GetTotalVotosPorEleccionAsync(eleccion.Id);

				var resumen = new ResumenElectoralDTO
				{
					EleccionId = eleccion.Id,
					NombreEleccion = eleccion.Nombre,
					FechaRealizacion = eleccion.FechaRealizacion,
					CantidadPartidos = cantidadPartidos,
					CantidadCandidatos = cantidadCandidatos,
					TotalVotos = totalVotos,
					Estado = eleccion.Estado
				};

				resumenList.Add(resumen);
			}

			return resumenList;
		}

		public async Task<(bool isValid, List<string> errors)> ValidarCreacionEleccionConAnioAsync(CrearEleccionDTO dto)
		{
			var errors = new List<string>();

			// Validar primero las reglas existentes
			var (isValidBase, baseErrors) = await ValidarCreacionEleccionAsync();
			if (!isValidBase)
			{
				errors.AddRange(baseErrors);
			}

			// Validar que no exista ya una elección para ese año
			var existeEleccionEnAnio = await _eleccionesRepository.ExisteEleccionEnAnioAsync(dto.Año);
			if (existeEleccionEnAnio)
			{
				errors.Add($"Ya existe una elección registrada para el año {dto.Año}. Solo se permite una elección por año.");
			}

			// Validar que el año del DTO coincida con el año de la fecha de realización
			if (dto.FechaRealizacion.Year != dto.Año)
			{
				errors.Add("El año especificado debe coincidir con el año de la fecha de realización.");
			}

			return (!errors.Any(), errors);
		}
	}
}