using AutoMapper;
using SADVO.Core.Application.Dtos.Alianzas;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;


namespace SADVO.Core.Application.Services
{
	public class AlianzasHelperService : IAlianzasHelperService
	{
		private readonly IAlianzasPoliticasServices _alianzasServices;
		private readonly IAlianzasPoliticasRepository _alianzasRepository;
		private readonly IPartidoPoliticoServices _partidosServices;
		private readonly IAsignacionCandidatosRepository _asignacionCandidatosRepository;
		private readonly IMapper _mapper;

		public AlianzasHelperService(
			IAlianzasPoliticasServices alianzasServices,
			IAlianzasPoliticasRepository alianzasRepository,
		IPartidoPoliticoServices partidosServices,
			IAsignacionCandidatosRepository asignacionCandidatosRepository,
			IMapper mapper)
		{
			_asignacionCandidatosRepository = asignacionCandidatosRepository;
			_alianzasServices = alianzasServices;
			_partidosServices = partidosServices;
			_alianzasRepository = alianzasRepository;
			_mapper = mapper;
		}

		public async Task<List<PartidosPoliticosDTO>> GetPartidosDisponiblesParaAlianzaAsync(int partidoId)
		{
			var todosLosPartidos = await _partidosServices.GetAllAsync();
			var partidosFiltrados = todosLosPartidos.Where(p => p.Id != partidoId).ToList();

			var relaciones = await _alianzasServices.GetAlianzasActivasAsync(partidoId);

			var idsRelacionados = relaciones
				.Select(r => r.PartidoSolicitanteId == partidoId ? r.PartidoSolicitanteId : r.PartidoSolicitanteId)
				.ToHashSet();

			var disponibles = partidosFiltrados
				.Where(p => !idsRelacionados.Contains(p.Id))
				.ToList();

			return _mapper.Map<List<PartidosPoliticosDTO>>(disponibles);
		}

		public async Task<bool> RomperAlianzaAsync(int alianzaId)
		{
			var alianza = await _alianzasRepository.GetByIdAsync(alianzaId);
			if (alianza == null) return false;

			var asignacionesRespaldadas = await _asignacionCandidatosRepository.GetRespaldosEntrePartidosAsync(
				alianza.PartidoSolicitanteId, alianza.PartidoDestinoId);

			foreach (var asignacion in asignacionesRespaldadas)
			{
				asignacion.PartidoRespaldaId = null;
			}

			alianza.EstadoAlianza = EstadoAlianza.Rota; 

			return true;
		}
	}

}
