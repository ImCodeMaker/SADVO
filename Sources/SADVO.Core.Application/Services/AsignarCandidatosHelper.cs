using AutoMapper;
using SADVO.Core.Application.Dtos.AsignacionCandidatos;
using SADVO.Core.Application.Dtos.Candidatos;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class AsignacionCandidatosHelperService : IAsignacionCandidatosHelperService
	{
		private readonly ICandidatosRepository _candidatosRepository;
		private readonly IPuestosElectivosRepository _puestosRepository;
		private readonly IAsignacionCandidatosRepository _asignacionRepository;
		private readonly IAlianzasPoliticasRepository _alianzasRepository;
		private readonly IMapper _mapper;

		public AsignacionCandidatosHelperService(
			ICandidatosRepository candidatosRepository,
			IPuestosElectivosRepository puestosRepository,
			IAsignacionCandidatosRepository asignacionRepository,
			IAlianzasPoliticasRepository alianzasRepository,
			IMapper mapper)
		{
			_candidatosRepository = candidatosRepository;
			_puestosRepository = puestosRepository;
			_asignacionRepository = asignacionRepository;
			_alianzasRepository = alianzasRepository;
			_mapper = mapper;
		}

		public async Task<List<AsignacionCandidatoDTO>> GetAsignacionesConAliadosAsync(int partidoId)
		{
			var asignaciones = await _asignacionRepository.GetAsignacionesConAliadosAsync(partidoId);
			return _mapper.Map<List<AsignacionCandidatoDTO>>(asignaciones);
		}

		public async Task<List<CandidatosDTO>> GetCandidatosDisponiblesAsync(int partidoId)
		{
			var candidatos = new List<CandidatosDTO>();

			// Candidatos del partido actual (sin cambios)
			var candidatosPartido = await _candidatosRepository.GetCandidatosActivosByPartidoAsync(partidoId);
			foreach (var candidato in candidatosPartido)
			{
				if (!await _asignacionRepository.ExisteCandidatoAsignadoEnPartidoAsync(candidato.Id, partidoId))
				{
					candidatos.Add(_mapper.Map<CandidatosDTO>(candidato));
				}
			}

			// NUEVA LÓGICA: Candidatos de partidos aliados para respaldar
			var alianzas = await _alianzasRepository.GetAlianzasActivasAsync(partidoId);
			foreach (var alianza in alianzas)
			{
				var partidoAliadoId = alianza.PartidoSolicitanteId == partidoId ?
					alianza.PartidoDestinoId : alianza.PartidoSolicitanteId;

				// Obtener candidatos ya asignados del partido aliado
				var candidatosAsignadosAliado = await _asignacionRepository.GetCandidatosAsignadosDePartidoAsync(partidoAliadoId);

				foreach (var candidatoAsignado in candidatosAsignadosAliado)
				{
					// Verificar que no esté ya respaldado por nuestro partido
					if (!await _asignacionRepository.ExisteRespaldoAsync(candidatoAsignado.CandidatoId, candidatoAsignado.PuestoElectivoId, partidoId))
					{
						candidatos.Add(_mapper.Map<CandidatosDTO>(candidatoAsignado.Candidato));
					}
				}
			}

			return candidatos.OrderBy(c => c.Nombre).ThenBy(c => c.Apellido).ToList();
		}

		public async Task<List<PuestoElectivoDTO>> GetPuestosDisponiblesAsync(int partidoId, int? candidatoId = null)
		{
			// Si no hay candidato, devolver los puestos normales
			if (!candidatoId.HasValue)
			{
				var puestosElectivos = await _puestosRepository.GetAllList();
				var puestosDisponibles = new List<PuestoElectivoDTO>();

				foreach (var puestos in puestosElectivos)
				{
					if (!await _asignacionRepository.ExistePuestoAsignadoEnPartidoAsync(puestos.Id, partidoId))
					{
						puestosDisponibles.Add(_mapper.Map<PuestoElectivoDTO>(puestos));
					}
				}

				return puestosDisponibles.OrderBy(p => p.Nombre).ToList();
			}

			// Si hay candidato, verificar si es de partido aliado
			var esDePartidoAliado = await CandidatoPerteneceAPartidoAliado(candidatoId.Value, partidoId);

			if (!esDePartidoAliado)
			{
				// Si no es de aliado, devolver los puestos normales
				return await GetPuestosDisponiblesAsync(partidoId);
			}

			// Obtener asignación original del candidato
			var asignacionOriginal = await _asignacionRepository.GetAsignacionDelCandidatoAsync(candidatoId.Value);

			if (asignacionOriginal == null)
			{
				return new List<PuestoElectivoDTO>(); // No tiene asignación original
			}

			// Verificar que ese puesto esté disponible en este partido
			var estaDisponible = !await _asignacionRepository.ExistePuestoAsignadoEnPartidoAsync(
				asignacionOriginal.PuestoElectivoId, partidoId);

			if (!estaDisponible)
			{
				return new List<PuestoElectivoDTO>();
			}

			// Obtener y retornar solo el puesto original
			var puesto = await _puestosRepository.GetById(asignacionOriginal.PuestoElectivoId);
			return new List<PuestoElectivoDTO> { _mapper.Map<PuestoElectivoDTO>(puesto) };
		}


		public async Task<bool> ValidarCandidatoParaAsignacionAsync(int candidatoId, int puestoElectivoId, int partidoId)
		{
			// Validar que no existe el candidato asignado en el mismo partido
			if (await _asignacionRepository.ExisteCandidatoAsignadoEnPartidoAsync(candidatoId, partidoId))
				return false;

			// Validar que no existe ya un candidato asignado a ese puesto en el partido
			if (await _asignacionRepository.ExistePuestoAsignadoEnPartidoAsync(puestoElectivoId, partidoId))
				return false;

			// Validar candidato de partido aliado
			var asignacionEnOtroPartido = await _asignacionRepository.GetAsignacionCandidatoEnOtroPartidoAsync(candidatoId, partidoId);
			if (asignacionEnOtroPartido != null && asignacionEnOtroPartido.PuestoElectivoId != puestoElectivoId)
				return false;

			return true;
		}

		public async Task<bool> CandidatoPerteneceAPartidoAliado(int candidatoId, int partidoId)
		{

			var alianzas = await _alianzasRepository.GetAlianzasActivasAsync(partidoId);

			var candidato = await _candidatosRepository.GetById(candidatoId);
			if (candidato == null)
				return false;

			foreach (var alianza in alianzas)
			{
				var aliadoId = alianza.PartidoSolicitanteId == partidoId
					? alianza.PartidoDestinoId
					: alianza.PartidoSolicitanteId;

				if (candidato.PartidoPoliticoId == aliadoId)
					return true;
			}

			return false;
		}


	}
}