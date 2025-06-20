using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IAsignacionDirigentesRepository : IGenericRepository<AsignacionDirigentes>
	{
		Task<AsignacionDirigentes?> GetByUserIdWithPartidoAsync(int userId);
	}
}
