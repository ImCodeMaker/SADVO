using SADVO.Core.Application.ViewModels.Usuarios;

namespace SADVO.Core.Application.Interfaces
{
	public interface IUserSession
	{
		UserViewModel GetUserSession();
		bool hasUser();
		bool checkRole();
	}
}