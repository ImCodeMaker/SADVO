using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;

namespace SADVO.Controllers
{
	public class AdminController : Controller
	{
		private readonly IUserSession _userSession;

		public AdminController(IUserSession userSession)
		{
			_userSession = userSession;
		}

		public IActionResult Index()
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}
			return View();
		}
	}
}
