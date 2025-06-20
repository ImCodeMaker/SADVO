using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IEleccionesRepository : IGenericRepository<Elecciones>
	{
		Task<Elecciones?> GetEleccionActivaAsync();
		Task<List<Elecciones>> GetAllEleccionesOrderedAsync();
		Task<bool> FinalizarEleccionAsync(int eleccionId);
		Task<Dictionary<int, int>> GetPartidosCountByEleccionAsync(int eleccionId);
		Task<Dictionary<int, int>> GetPuestosCountByEleccionAsync(int eleccionId);
		Task<List<ResultadoEleccion>> GetResultadosEleccionAsync(int eleccionId);
		Task<List<int>> GetAniosDisponiblesAsync();
		Task<List<Elecciones>> GetEleccionesPorAnioAsync(int anio);
		Task<int> GetCantidadPartidosPorEleccionAsync(int eleccionId);
		Task<int> GetCantidadCandidatosPorEleccionAsync(int eleccionId);
		Task<int> GetTotalVotosPorEleccionAsync(int eleccionId);
		Task<bool> ExisteEleccionEnAnioAsync(int anio);
		Task<bool> ExisteEleccionActivaAsync();
	}
}