using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IAlianzasPoliticasRepository : IGenericRepository<AlianzasPoliticas>
	{
		Task<List<AlianzasPoliticas>> GetSolicitudesPendientesAsync(int partidoId);
		Task<List<AlianzasPoliticas>> GetSolicitudesEnviadasAsync(int partidoId);
		Task<List<AlianzasPoliticas>> GetAlianzasActivasAsync(int partidoId);
		Task<AlianzasPoliticas?> GetByIdAsync(int id);
		Task<bool> ExisteRelacionPendienteAsync(int solicitanteId, int destinoId);
	}
}
