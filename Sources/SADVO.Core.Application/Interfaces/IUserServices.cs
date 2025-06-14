using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IUserServices : IGenericServices<UsuarioDTO, Usuarios>
	{
		Task AddAdminUser();
		Task<bool> CreateAdminUser(CrearUsuarioDTO crearUsuarioDTO);
		Task<UsuarioDTO> LoginAsync(LoginDto loginDto);
	}
}
