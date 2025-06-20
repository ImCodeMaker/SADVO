using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IAsignacionCandidatosRepository : IGenericRepository<AsignacionCandidatos>
	{
		Task<List<AsignacionCandidatos>> GetAsignacionesByPartidoAsync(int partidoId);
		Task<List<AsignacionCandidatos>> GetAsignacionesActivasAsync(int partidoId);
		Task<bool> ExisteCandidatoAsignadoEnPartidoAsync(int candidatoId, int partidoId);
		Task<bool> ExistePuestoAsignadoEnPartidoAsync(int puestoElectivoId, int partidoId);
		Task<AsignacionCandidatos?> GetAsignacionCandidatoEnOtroPartidoAsync(int candidatoId, int partidoIdActual);
		Task<AsignacionCandidatos?> GetByIdWithIncludesAsync(int id);
		Task<List<AsignacionCandidatos>> GetCandidatosAsignadosDePartidoAsync(int partidoId);
		Task<AsignacionCandidatos?> GetAsignacionOriginalAsync(int candidatoId, int puestoId);
		Task<bool> ExisteRespaldoAsync(int candidatoId, int puestoElectivoId, int partidoRespaldaId);
		Task<bool> EliminarRespaldosAsync(int candidatoId, int puestoElectivoId);
		Task<List<AsignacionCandidatos>> GetRespaldosAsync(int candidatoId, int puestoElectivoId);
		Task<List<int>> GetCandidatosNoDisponiblesParaPartidoAsync(int partidoId);
		Task<List<int>> GetPuestosNoDisponiblesParaPartidoAsync(int partidoId);
		Task<List<AsignacionCandidatos>> GetRespaldosEntrePartidosAsync(int partidoId1, int partidoId2);
		Task<AsignacionCandidatos?> GetAsignacionDelCandidatoAsync(int candidatoId);
		Task<List<AsignacionCandidatos>> GetAsignacionesConAliadosAsync(int partidoId);

	}
}