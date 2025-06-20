using AutoMapper;
using SADVO.Core.Application.Dtos.Alianzas;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class AlianzasPoliticasServices : GenericService<CrearAlianzaDTO, UpdateAlianzaDTO, AlianzasDTO, AlianzasPoliticas>, IAlianzasPoliticasServices
	{
		private new readonly IAlianzasPoliticasRepository _repository;
		private new readonly IMapper _mapper;

		public AlianzasPoliticasServices(IAlianzasPoliticasRepository repository, IMapper mapper)
			: base(repository, mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<List<AlianzasDTO>> GetSolicitudesPendientesAsync(int partidoId)
		{
			var entities = await _repository.GetSolicitudesPendientesAsync(partidoId);
			return _mapper.Map<List<AlianzasDTO>>(entities);
		}

		public async Task<List<AlianzasDTO>> GetSolicitudesEnviadasAsync(int partidoId)
		{
			var entities = await _repository.GetSolicitudesEnviadasAsync(partidoId);
			return _mapper.Map<List<AlianzasDTO>>(entities);
		}

		public async Task<List<AlianzasDTO>> GetAlianzasActivasAsync(int partidoId)
		{
			var entities = await _repository.GetAlianzasActivasAsync(partidoId);
			return _mapper.Map<List<AlianzasDTO>>(entities);
		}

		public async Task<bool> AceptarSolicitudAsync(int id)
		{
			var solicitud = await _repository.GetByIdAsync(id);
			if (solicitud == null || solicitud.EstadoAlianza != EstadoAlianza.EnEspera)
				return false;

			solicitud.EstadoAlianza = EstadoAlianza.Aceptada;
			solicitud.FechaRespuesta = DateTime.Now;

			await _repository.UpdateAsync(id, solicitud);
			return true;
		}

		public async Task<bool> RechazarSolicitudAsync(int id)
		{
			var solicitud = await _repository.GetByIdAsync(id);
			if (solicitud == null || solicitud.EstadoAlianza != EstadoAlianza.EnEspera)
				return false;

			solicitud.EstadoAlianza = EstadoAlianza.Rechazada;
			solicitud.FechaRespuesta = DateTime.Now;

			await _repository.UpdateAsync(id, solicitud);
			return true;
		}

		public async Task<bool> EliminarSolicitudAsync(int id)
		{
			var solicitud = await _repository.GetByIdAsync(id);
			if (solicitud == null)
				return false;

			// Borrado lógico
			await _repository.UpdateEstadoAsync(id, false);
			return true;
		}

		public async Task<bool> ExisteRelacionPendienteAsync(int solicitanteId, int destinoId)
		{
			return await _repository.ExisteRelacionPendienteAsync(solicitanteId, destinoId);
		}

		public override async Task<bool> AddAsync(CrearAlianzaDTO createDto)
		{
			if (createDto == null) throw new ArgumentNullException(nameof(createDto));

			// Validar que no exista una relación pendiente
			if (await ExisteRelacionPendienteAsync(createDto.PartidoSolicitanteId, createDto.PartidoDestinoId))
				return false;

			// Validar que no sea el mismo partido
			if (createDto.PartidoSolicitanteId == createDto.PartidoDestinoId)
				return false;

			return await base.AddAsync(createDto);
		}
	}
}