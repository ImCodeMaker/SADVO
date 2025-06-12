using SADVO.Core.Application.Dtos;

namespace SADVO.Core.Application.Interfaces
{
	public interface IUserServices
	{
		Task addAdminUser();
		Task<bool> CreateAdminUser(CrearUsuarioDTO crearUsuarioDTO);
		Task<bool> AddAsync(CrearUsuarioDTO crearUsuarioDTO);
		Task<bool> UpdateUserAsync(int Id, UsuarioDTO usuarioDTO);
		Task<bool> DeleteUserAsync(int Id);
		Task<List<UsuarioDTO>> GetAllUsers();
		Task<UsuarioDTO> LoginAsync(LoginDto loginDto);
	}
}
