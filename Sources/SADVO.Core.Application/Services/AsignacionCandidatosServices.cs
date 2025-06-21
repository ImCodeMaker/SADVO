using AutoMapper;
using SADVO.Core.Application.Dtos.AsignacionCandidatos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class AsignacionCandidatosServices : GenericService<CrearAsignacionCandidatoDTO, UpdateAsignacionCandidatoDTO, AsignacionCandidatoDTO, AsignacionCandidatos>,
											  IAsignacionCandidatosServices
	{
		private new readonly IAsignacionCandidatosRepository _repository;

		public AsignacionCandidatosServices(IAsignacionCandidatosRepository repository, IMapper mapper)
			: base(repository, mapper)
		{
			_repository = repository;
		}

		public override async Task<bool> AddAsync(CrearAsignacionCandidatoDTO dto)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			// Verificar y limpiar asignaciones de puestos inactivos antes de cualquier operación
			await _repository.LiberarAsignacionesDePuestosInactivosManualAsync();

			if (dto.EsRespaldo)
			{
				var asignacionOriginal = await _repository.GetAsignacionOriginalAsync(dto.CandidatoId, dto.PuestoElectivoId);
				if (asignacionOriginal == null)
					throw new InvalidOperationException("No se puede respaldar un candidato que no está asignado originalmente a este puesto electivo.");

				if (asignacionOriginal.PartidoPoliticoId == dto.PartidoPoliticoId)
					throw new InvalidOperationException("Un partido no puede respaldar a su propio candidato.");

				if (await _repository.ExisteRespaldoAsync(dto.CandidatoId, dto.PuestoElectivoId, dto.PartidoPoliticoId))
					throw new InvalidOperationException("Este partido ya respalda a este candidato para este puesto electivo.");

				// CORRECCIÓN: No cambiar el PartidoPoliticoId, solo asignar el PartidoRespaldaId
				dto.PartidoRespaldaId = dto.PartidoPoliticoId;
				dto.PartidoPoliticoId = asignacionOriginal.PartidoPoliticoId; // El partido original del candidato
			}
			else
			{
				if (await _repository.ExisteCandidatoAsignadoEnPartidoAsync(dto.CandidatoId, dto.PartidoPoliticoId))
					throw new InvalidOperationException("Este candidato ya está asignado a un puesto dentro del partido.");

				if (await _repository.ExistePuestoAsignadoEnPartidoAsync(dto.PuestoElectivoId, dto.PartidoPoliticoId))
					throw new InvalidOperationException("Ya existe un candidato asignado a este puesto en el partido.");

				var asignacionEnOtroPartido = await _repository.GetAsignacionCandidatoEnOtroPartidoAsync(dto.CandidatoId, dto.PartidoPoliticoId);
				if (asignacionEnOtroPartido != null)
				{
					if (asignacionEnOtroPartido.PuestoElectivoId != dto.PuestoElectivoId)
						throw new InvalidOperationException("Este candidato en su partido de origen aspira a un puesto diferente al seleccionado.");
				}

				dto.PartidoRespaldaId = null;
			}

			return await base.AddAsync(dto);
		}

		public async Task<List<AsignacionCandidatoDTO>> GetAsignacionesByPartidoAsync(int partidoId)
		{
			var asignaciones = await _repository.GetAsignacionesByPartidoAsync(partidoId);
			return _mapper.Map<List<AsignacionCandidatoDTO>>(asignaciones);
		}

		public async Task<List<AsignacionCandidatoDTO>> GetAsignacionesActivasAsync(int partidoId)
		{
			var asignaciones = await _repository.GetAsignacionesActivasAsync(partidoId);
			return _mapper.Map<List<AsignacionCandidatoDTO>>(asignaciones);
		}

		public async Task<List<AsignacionCandidatoDTO>> GetAsignacionesOriginalesAsync(int partidoId)
		{
			var asignaciones = await _repository.GetCandidatosAsignadosDePartidoAsync(partidoId);
			return _mapper.Map<List<AsignacionCandidatoDTO>>(asignaciones);
		}

		public async Task<bool> ValidarAsignacionCandidatoAsync(int candidatoId, int puestoElectivoId, int partidoId)
		{
			// Limpiar asignaciones inactivas antes de validar
			await _repository.LiberarAsignacionesDePuestosInactivosManualAsync();

			if (await _repository.ExisteCandidatoAsignadoEnPartidoAsync(candidatoId, partidoId))
				return false;

			if (await _repository.ExistePuestoAsignadoEnPartidoAsync(puestoElectivoId, partidoId))
				return false;

			var asignacionEnOtroPartido = await _repository.GetAsignacionCandidatoEnOtroPartidoAsync(candidatoId, partidoId);
			if (asignacionEnOtroPartido != null && asignacionEnOtroPartido.PuestoElectivoId != puestoElectivoId)
				return false;

			return true;
		}

		public async Task<bool> PuedeRespaldarseCandidatoAsync(int candidatoId, int puestoElectivoId, int partidoRespaldaId)
		{
			// Limpiar asignaciones inactivas antes de validar
			await _repository.LiberarAsignacionesDePuestosInactivosManualAsync();

			var asignacionOriginal = await _repository.GetAsignacionOriginalAsync(candidatoId, puestoElectivoId);
			if (asignacionOriginal == null)
				return false;

			if (asignacionOriginal.PartidoPoliticoId == partidoRespaldaId)
				return false;

			if (await _repository.ExisteRespaldoAsync(candidatoId, puestoElectivoId, partidoRespaldaId))
				return false;

			return true;
		}

		public async Task<AsignacionCandidatoDTO?> GetByIdWithIncludesAsync(int id)
		{
			var asignacion = await _repository.GetByIdWithIncludesAsync(id);
			return _mapper.Map<AsignacionCandidatoDTO>(asignacion);
		}

		// Métodos adicionales para funcionalidad de limpieza
		public async Task<List<AsignacionCandidatoDTO>> GetAsignacionesConPuestosInactivosAsync()
		{
			var asignaciones = await _repository.GetAsignacionesConPuestosInactivosAsync();
			return _mapper.Map<List<AsignacionCandidatoDTO>>(asignaciones);
		}

		public async Task<bool> LiberarAsignacionesDePuestosInactivosAsync()
		{
			try
			{
				return await _repository.LiberarAsignacionesDePuestosInactivosManualAsync();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Error al liberar asignaciones de puestos inactivos: {ex.Message}");
			}
		}

		public async Task<bool> DesactivarAsignacionesPorPuestoAsync(int puestoElectivoId)
		{
			try
			{
				return await _repository.DesactivarAsignacionesPorPuestoInactivoAsync(puestoElectivoId);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Error al desactivar asignaciones del puesto {puestoElectivoId}: {ex.Message}");
			}
		}

		public async Task<List<int>> GetCandidatosNoDisponiblesParaPartidoAsync(int partidoId)
		{
			return await _repository.GetCandidatosNoDisponiblesParaPartidoAsync(partidoId);
		}

		public async Task<List<int>> GetPuestosNoDisponiblesParaPartidoAsync(int partidoId)
		{
			return await _repository.GetPuestosNoDisponiblesParaPartidoAsync(partidoId);
		}

		public async Task<List<AsignacionCandidatoDTO>> GetAsignacionesConAliadosAsync(int partidoId)
		{
			var asignaciones = await _repository.GetAsignacionesConAliadosAsync(partidoId);
			return _mapper.Map<List<AsignacionCandidatoDTO>>(asignaciones);
		}
	}
}