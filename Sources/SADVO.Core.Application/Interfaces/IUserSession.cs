using SADVO.Core.Application.ViewModels;

namespace SADVO.Core.Application.Interfaces
{
	public interface IUserSession
	{
		UserViewModel GetUserSession();
		bool hasUser();
	}
}