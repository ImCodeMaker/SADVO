using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;

namespace SADVO.Controllers
{
	public class DirigenteController : Controller
	{
		private readonly IUserSession _userSession;

		public DirigenteController(IUserSession userSession)
		{
			_userSession = userSession;
		}

		public IActionResult Index()
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}
			return View();
		}
	}
}
