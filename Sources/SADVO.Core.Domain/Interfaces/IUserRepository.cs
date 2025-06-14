using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IUserRepository : IGenericRepository<Usuarios>
	{
		Task<Usuarios?> LoginAsync(string username, string password);
	}
}
