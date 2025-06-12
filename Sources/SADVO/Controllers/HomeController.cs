using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;
using SADVO.Models;
using System.Diagnostics;

namespace SADVO.Controllers
{
	public class HomeController : Controller
	{
		private readonly IUserServices _userServices;

		public HomeController(IUserServices userServices)
		{
			_userServices = userServices;
		}

		public async Task<IActionResult> Index()
		{
			await _userServices.addAdminUser();
			return View();
		}

		public IActionResult Votaciones()
		{
			return View();
		}
	}
}
