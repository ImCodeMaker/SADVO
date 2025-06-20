using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IUserServices : IGenericServices<CrearUsuarioDTO,UpdateUsuarioDTO,UsuarioDTO, Usuarios>
	{
		Task AddAdminUser();
		Task<bool> CreateAdminUser(CrearUsuarioDTO crearUsuarioDTO);
		Task<List<UsuarioDTO>> GetActiveUsersAsync();
		Task<UsuarioDTO> LoginAsync(LoginDto loginDto);
	}
}
