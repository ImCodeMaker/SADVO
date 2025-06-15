using SADVO.Core.Application.ViewModels.Usuarios;

namespace SADVO.Core.Application.Interfaces
{
	public interface IUserSession
	{
		UsuarioViewModel GetUserSession();
		bool hasUser();
		bool checkRole();
	}
}