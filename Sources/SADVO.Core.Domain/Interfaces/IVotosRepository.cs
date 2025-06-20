using SADVO.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IVotosRepository : IGenericRepository<Votos>
	{
		Task<List<Votos>> GetVotosByEleccionAsync(int eleccionId);
		Task<bool> CiudadanoHaVotadoAsync(int eleccionId, int ciudadanoId);
		Task RegistrarVotoAsync(Votos voto, int ciudadanoId);
		Task<List<(PuestosElectivos Puesto, List<Candidatos> Candidatos)>> GetCandidatosParaVotacionAsync(int eleccionId);
		Task<List<PartidosPoliticos>> GetPartidosParaCandidatoAsync(int candidatoId, int puestoElectivoId);
		Task<bool> EsRespaldoAsync(int candidatoId, int puestoElectivoId, int partidoId);
		Task<int> GetTotalVotosPorEleccionAsync(int eleccionId);
		Task<int> GetTotalVotosPorCandidatoAsync(int eleccionId, int candidatoId);
	}
}