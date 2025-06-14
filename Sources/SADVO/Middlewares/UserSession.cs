using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Usuarios;

namespace SADVO.Middlewares
{
	public class UserSession : IUserSession
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserSession(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public bool hasUser()
		{
			UserViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UserViewModel>("User");

			if (userViewModel == null)
			{
				return false;
			}
			return true;
		}

		public UserViewModel GetUserSession()
		{
			UserViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UserViewModel>("User");

			if (userViewModel == null)
			{
				return null!;
			}
			return userViewModel;
		}

		//public bool IsAdmin()
		//{
		//	UserViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UserViewModel>("User");

		//	if (userViewModel == null)
		//	{
		//		return false!; 
		//	}
		//	return userViewModel.Rol == "Administrador";
		//}

		//public bool IsDirigente()
		//{
		//	UserViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UserViewModel>("User");

		//	if (userViewModel == null)
		//	{
		//		return false!;
		//	}
		//	return userViewModel.Rol == "Dirigente";
		//}
		
		public bool checkRole()
		{
			UserViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UserViewModel>("User");

			if (userViewModel == null)
			{
				return false!;
			}

			if (userViewModel.Rol == "Administrador")
			{
				return true;
			}

			return false;
		}
	}
}
