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

			if (dto.EsRespaldo)
			{
				// LÓGICA PARA RESPALDOS
				var asignacionOriginal = await _repository.GetAsignacionOriginalAsync(dto.CandidatoId, dto.PuestoElectivoId);
				if (asignacionOriginal == null)
					throw new InvalidOperationException("No se puede respaldar un candidato que no está asignado originalmente a este puesto electivo.");

				// Verificar que el partido que respalda no sea el mismo que el original
				if (asignacionOriginal.PartidoPoliticoId == dto.PartidoPoliticoId)
					throw new InvalidOperationException("Un partido no puede respaldar a su propio candidato.");

				// Verificar que este partido no haya respaldado ya a este candidato para este puesto
				if (await _repository.ExisteRespaldoAsync(dto.CandidatoId, dto.PuestoElectivoId, dto.PartidoPoliticoId))
					throw new InvalidOperationException("Este partido ya respalda a este candidato para este puesto electivo.");

				// Configurar correctamente el respaldo
				dto.PartidoPoliticoId = asignacionOriginal.PartidoPoliticoId; // Mantener el partido original
				dto.PartidoRespaldaId = dto.PartidoPoliticoId; // El que estaba intentando asignar ahora es el respaldo
			}
			else
			{
				// LÓGICA PARA ASIGNACIONES ORIGINALES

				// Verificar que el candidato no esté ya asignado a algún puesto en este partido
				if (await _repository.ExisteCandidatoAsignadoEnPartidoAsync(dto.CandidatoId, dto.PartidoPoliticoId))
					throw new InvalidOperationException("Este candidato ya está asignado a un puesto dentro del partido.");

				// Verificar que no haya ya un candidato asignado a este puesto en este partido
				if (await _repository.ExistePuestoAsignadoEnPartidoAsync(dto.PuestoElectivoId, dto.PartidoPoliticoId))
					throw new InvalidOperationException("Ya existe un candidato asignado a este puesto en el partido.");

				// Verificar regla de alianzas: si el candidato está en otro partido, debe aspirar al mismo puesto
				var asignacionEnOtroPartido = await _repository.GetAsignacionCandidatoEnOtroPartidoAsync(dto.CandidatoId, dto.PartidoPoliticoId);
				if (asignacionEnOtroPartido != null)
				{
					if (asignacionEnOtroPartido.PuestoElectivoId != dto.PuestoElectivoId)
						throw new InvalidOperationException("Este candidato en su partido de origen aspira a un puesto diferente al seleccionado.");
				}

				// Para asignaciones originales, no hay partido que respalda
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
			// Validar que no existe el candidato asignado en el mismo partido
			if (await _repository.ExisteCandidatoAsignadoEnPartidoAsync(candidatoId, partidoId))
				return false;

			// Validar que no existe ya un candidato asignado a ese puesto en el partido
			if (await _repository.ExistePuestoAsignadoEnPartidoAsync(puestoElectivoId, partidoId))
				return false;

			// Validar candidato de partido aliado
			var asignacionEnOtroPartido = await _repository.GetAsignacionCandidatoEnOtroPartidoAsync(candidatoId, partidoId);
			if (asignacionEnOtroPartido != null && asignacionEnOtroPartido.PuestoElectivoId != puestoElectivoId)
				return false;

			return true;
		}

		public async Task<bool> PuedeRespaldarseCandidatoAsync(int candidatoId, int puestoElectivoId, int partidoRespaldaId)
		{
			// Verificar que existe una asignación original para este candidato y puesto
			var asignacionOriginal = await _repository.GetAsignacionOriginalAsync(candidatoId, puestoElectivoId);
			if (asignacionOriginal == null)
				return false;

			// Verificar que el partido que respalda no sea el mismo que el original
			if (asignacionOriginal.PartidoPoliticoId == partidoRespaldaId)
				return false;

			// Verificar que este partido no haya respaldado ya a este candidato
			if (await _repository.ExisteRespaldoAsync(candidatoId, puestoElectivoId, partidoRespaldaId))
				return false;

			return true;
		}

		public async Task<AsignacionCandidatoDTO?> GetByIdWithIncludesAsync(int id)
		{
			var asignacion = await _repository.GetByIdWithIncludesAsync(id);
			return _mapper.Map<AsignacionCandidatoDTO>(asignacion);
		}



		//public async Task<bool> EliminarAsignacionAsync(int id)
		//{
		//	if (await ExisteEleccionActivaAsync())
		//		throw new InvalidOperationException("No se puede eliminar una asignación mientras exista una elección activa.");

		//	var asignacion = await _repository.GetById(id);
		//	if (asignacion == null)
		//		throw new InvalidOperationException("La asignación no existe.");

		//	// Eliminar también todos los respaldos asociados si es una asignación original
		//	if (asignacion.PartidoRespaldaId == null)
		//	{
		//		await _repository.EliminarRespaldosAsync(asignacion.CandidatoId, asignacion.PuestoElectivoId);
		//	}

		//	return await _repository.(id);
		//}
	}
}