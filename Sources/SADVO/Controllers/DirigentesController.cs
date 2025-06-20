using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.Services;
using System.Threading.Tasks;

namespace SADVO.Controllers
{
	public class DirigenteController : Controller
	{
		private readonly IUserSession _userSession;
		private readonly IAsignacionDirigentesServices _asignacionDirigenteServices;

		public DirigenteController(IUserSession userSession, IAsignacionDirigentesServices asignacionDirigentesServices)
		{
			_userSession = userSession;
			_asignacionDirigenteServices = asignacionDirigentesServices;
		}

		private async Task<IActionResult?> ValidateUserAndPartido()
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AccessDenied" });
			}

			var currentSession = _userSession.GetUserSession();
			var userId = currentSession!.Id;

			try
			{
				await _asignacionDirigenteServices.GetDirigente(userId);
				return null;
			}
			catch (InvalidOperationException)
			{
				return RedirectToRoute(new { controller = "Auth", action = "NoPartidoPolitico" });
			}
		}

		public async Task<IActionResult> Index()
		{
			var validationResult = await ValidateUserAndPartido();
			if (validationResult != null)
			{
				return validationResult; 
			}

			return View();
		}
	}
}