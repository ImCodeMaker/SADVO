using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.Services;
using SADVO.Core.Application.ViewModels;
using SADVO.Core.Application.Helpers;

namespace SADVO.Controllers
{
	public class HomeController : Controller
	{
		private readonly IUserServices _userServices;
		private readonly ICiudadanosServices _ciudadanosServices;
		private readonly ICiudadanoSession _ciudadanoSession;

		public HomeController(IUserServices userServices, ICiudadanosServices ciudadanosServices, ICiudadanoSession ciudadanoSession)
		{
			_userServices = userServices;
			_ciudadanosServices = ciudadanosServices;
			_ciudadanoSession = ciudadanoSession;
		}

		public async Task<IActionResult> Index()
		{
			await _userServices.AddAdminUser();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(string numeroCedula)
		{
			if (string.IsNullOrWhiteSpace(numeroCedula))
			{
				ViewBag.Error = "Por favor, ingresa tu número de cédula";
				return View();
			}

			if (!EsCedulaValida(numeroCedula))
			{
				ViewBag.Error = "El formato de cédula no es válido. Ejemplo: 001-0000000-1";
				return View();
			}

			TempData["CedulaIngresada"] = numeroCedula;

			return RedirectToAction("ScanCedula");
		}

		public IActionResult Votaciones(string cedula)
		{
			if (string.IsNullOrWhiteSpace(cedula))
			{
				TempData["Error"] = "Error: Tienes que ingresar una cédula antes de estar aquí.";
				return RedirectToAction("Index");
			}

			ViewBag.Cedula = cedula;
			return View();
		}

		[HttpGet]
		public IActionResult ScanCedula()
		{
			if (TempData["CedulaIngresada"] == null)
			{
				TempData["Error"] = "Error: Tienes que ingresar una cédula antes de estar aquí.";
				return RedirectToAction("Index");
			}

			TempData.Keep("CedulaIngresada");

			var viewModel = new CedulaScanViewModel();
			ViewBag.CedulaIngresada = TempData["CedulaIngresada"].ToString();

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ScanCedula(CedulaScanViewModel model)
		{
			if (TempData["CedulaIngresada"] == null)
			{
				TempData["Error"] = "Error: Tienes que ingresar una cédula antes de estar aquí.";
				return RedirectToAction("Index");
			}

			var cedulaIngresada = TempData["CedulaIngresada"].ToString();
			TempData.Keep("CedulaIngresada");

			if (!ModelState.IsValid)
			{
				ViewBag.CedulaIngresada = cedulaIngresada;
				return View(model);
			}

			try
			{
				var resultado = await CedulaOCRService.ExtraerCedulaAsync(model.CedulaImageFile);


				if (resultado.Exitoso)
				{
					var cedulaEscaneadaLimpia = LimpiarCedula(resultado.NumeroCedula);
					var cedulaIngresadaLimpia = LimpiarCedula(cedulaIngresada);

					if (cedulaEscaneadaLimpia == cedulaIngresadaLimpia)
					{
						var ciudadano = await _ciudadanosServices.GetCiudadanoByCedula(cedulaIngresada);

						if(ciudadano != null)
						{
							HttpContext.Session.Set("Ciudadano", ciudadano);
						}
						TempData.Remove("CedulaIngresada");
						return RedirectToAction("Index","Votacion");
					}
					else
					{
						ViewBag.Error = $"La cédula escaneada ({resultado.NumeroCedula}) no coincide con la cédula ingresada ({cedulaIngresada}). Por favor, verifica que sea tu cédula.";
						ViewBag.CedulaIngresada = cedulaIngresada;
						return View(model);
					}
				}
				else
				{
					ViewBag.Error = $"No se pudo leer la cédula correctamente: {resultado.ErrorMessage}";
					ViewBag.CedulaIngresada = cedulaIngresada;
					return View(model);
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Error procesando imagen: {ex.Message}";
				ViewBag.CedulaIngresada = cedulaIngresada;
				return View(model);
			}
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ConfirmarCedula(ResultadosOCRViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("ResultadosOCR", model);
			}

			if (!EsCedulaValida(model.CedulaConfirmada))
			{
				ModelState.AddModelError("CedulaConfirmada", "El formato de cédula no es válido. Ejemplo: 001-0000000-1");
				return View("ResultadosOCR", model);
			}

			return RedirectToAction("Votaciones", new { cedula = model.CedulaConfirmada });
		}

		private bool EsCedulaValida(string cedula)
		{
			if (string.IsNullOrWhiteSpace(cedula))
				return false;

			var soloNumeros = cedula.Replace("-", "");

			if (soloNumeros.Length != 11)
				return false;

			if (!System.Text.RegularExpressions.Regex.IsMatch(soloNumeros, @"^\d{11}$"))
				return false;

			if (System.Text.RegularExpressions.Regex.IsMatch(soloNumeros, @"^(\d)\1{10}$"))
				return false;

			return true;
		}

		private string LimpiarCedula(string cedula)
		{
			if (string.IsNullOrWhiteSpace(cedula))
				return string.Empty;

			return cedula.Replace("-", "").Replace(" ", "").Trim();
		}
	}
}