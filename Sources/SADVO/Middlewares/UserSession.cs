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
			UsuarioViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UsuarioViewModel>("User");

			if (userViewModel == null)
			{
				return false;
			}
			return true;
		}

		public UsuarioViewModel GetUserSession()
		{
			UsuarioViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UsuarioViewModel>("User");

			if (userViewModel == null)
			{
				return null!;
			}
			return userViewModel;
		}

		
		public bool checkRole()
		{
			UsuarioViewModel? userViewModel = _httpContextAccessor.HttpContext?.Session.Get<UsuarioViewModel>("User");

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
