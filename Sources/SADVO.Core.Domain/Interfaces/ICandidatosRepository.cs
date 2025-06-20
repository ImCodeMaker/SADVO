using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface ICandidatosRepository : IGenericRepository<Candidatos>
	{
		Task<Candidatos?> GetCandidatoConAsignacionesAsync(int candidatoId);
		Task<List<Candidatos>> GetCandidatosActivosByPartidoAsync(int partidoId);
	}
}
