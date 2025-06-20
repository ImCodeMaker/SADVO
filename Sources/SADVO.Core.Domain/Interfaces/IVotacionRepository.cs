using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IVotacionRepository
	{
		Task<List<AsignacionCandidatos>> GetCandidatosParaVotacionAsync(int eleccionId);
		Task<List<AsignacionCandidatos>> GetRespaldosParaCandidatoAsync(int candidatoId, int puestoElectivoId);
		Task<bool> YaVotoEnEleccionAsync(int ciudadanoId, int eleccionId);
		Task<bool> YaVotoEnPuestoAsync(int ciudadanoId, int eleccionId, int puestoElectivoId);
		Task<bool> RegistrarVotoAsync(Votos voto);
		Task<bool> RegistrarHistorialVotacionAsync(HistorialVotaciones historial);
		Task<Elecciones?> GetEleccionActivaAsync();
		Task<List<int>> GetPuestosVotadosPorCiudadanoAsync(int ciudadanoId, int eleccionId);
	}
}