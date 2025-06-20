using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Usuarios;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Dtos.Usuarios;

namespace SADVO.Controllers
{
	public class AuthController : Controller
	{
		private readonly IUserServices _userServices;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;

		public AuthController(IUserServices userServices, IMapper mapper, IUserSession userSession)
		{
			_userServices = userServices;
			_mapper = mapper;
			_userSession = userSession;
		}

		public IActionResult Index()
		{
			if (_userSession.hasUser())
			{
				var userSession = _userSession.GetUserSession();

				if (userSession != null)
				{
					return userSession.Rol switch
					{
						"Administrador" => RedirectToRoute(new { controller = "Admin", action = "Index" }),
						"Dirigente" => RedirectToRoute(new { controller = "Dirigente", action = "Index" }),
						_ => RedirectToRoute(new { controller = "Auth", action = "Index" }),
					};
				}
			}

			return View(new LoginViewModel { NombreUsuario = "", Contraseña = "" });
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmSignUp(LoginViewModel model)
		{
			if (_userSession.hasUser())
			{
				var userSession = _userSession.GetUserSession();
				if (userSession != null)
				{
					return userSession.Rol switch
					{
						"Administrador" => RedirectToRoute(new { controller = "Admin", action = "Index" }),
						"Dirigente" => RedirectToRoute(new { controller = "Dirigente", action = "Index" }),
						_ => RedirectToRoute(new { controller = "Auth", action = "Index" }),
					};
				}
			}

			if (!ModelState.IsValid)
			{
				return View("Index", model);
			}

			var loginDto = _mapper.Map<LoginDto>(model);

			try
			{
				var loggedUser = await _userServices.LoginAsync(loginDto);

				if (loggedUser != null)
				{
					var userViewModel = _mapper.Map<UsuarioViewModel>(loggedUser);

					HttpContext.Session.Set("User", userViewModel);
					TempData["Message"] = "Login Exitoso";

					return userViewModel.Rol switch
					{
						"Administrador" => RedirectToRoute(new { controller = "Admin", action = "Index" }),
						"Dirigente" => RedirectToRoute(new { controller = "Dirigente", action = "Index" }),
						_ => RedirectToAction("Index")
					};
				}
			}
			catch (Exception ex)
			{
				if (ex.Message == "Usuario inactivo")
				{
					TempData["ErrorMessage"] = "Este usuario está inactivo.";
				}
				else
				{
					TempData["ErrorMessage"] = "Error al iniciar sesión.";
				}

				return RedirectToAction("Index");
			}

			TempData["ErrorMessage"] = "Credenciales incorrectas, intente de nuevo.";
			return RedirectToAction("Index");
		}


		public IActionResult Logout()
		{
			HttpContext.Session.Remove("User");
			HttpContext.Session.Clear();
			return RedirectToRoute(new { controller = "Auth", action = "Index" });
		}

		public IActionResult AcessDenied()
		{
			if (_userSession.hasUser())
			{
				return View();
			}
			return RedirectToRoute(new { controller = "Auth", action = "Index" });
		}

		public IActionResult PeriodoElectoral()
		{
			if (_userSession.hasUser())
			{
				return View();
			}
			return RedirectToRoute(new { controller = "Auth", action = "Index" });
		}
		public IActionResult NoPartidoPolitico()
		{
			if (_userSession.hasUser())
			{
				return View();
			}
			return RedirectToRoute(new { controller = "Auth", action = "Index" });
		}
	}

}
