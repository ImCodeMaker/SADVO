using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.Services;
using SADVO.Core.Application.ViewModels;

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
			await _userServices.AddAdminUser();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(string numeroCedula)
		{
			if (string.IsNullOrWhiteSpace(numeroCedula))
			{
				ViewBag.Error = "Por favor, ingresa tu n�mero de c�dula";
				return View();
			}

			// Validar formato de c�dula
			if (!EsCedulaValida(numeroCedula))
			{
				ViewBag.Error = "El formato de c�dula no es v�lido. Ejemplo: 001-0000000-1";
				return View();
			}

			// Guardar c�dula en TempData para el flujo de escaneo
			TempData["CedulaIngresada"] = numeroCedula;

			// Redirigir a escaneo de c�dula
			return RedirectToAction("ScanCedula");
		}

		public IActionResult Votaciones(string cedula)
		{
			if (string.IsNullOrWhiteSpace(cedula))
			{
				TempData["Error"] = "Error: Tienes que ingresar una c�dula antes de estar aqu�.";
				return RedirectToAction("Index");
			}

			ViewBag.Cedula = cedula;
			return View();
		}

		[HttpGet]
		public IActionResult ScanCedula()
		{
			// Verificar que el usuario haya ingresado una c�dula primero
			if (TempData["CedulaIngresada"] == null)
			{
				TempData["Error"] = "Error: Tienes que ingresar una c�dula antes de estar aqu�.";
				return RedirectToAction("Index");
			}

			// Mantener la c�dula para la siguiente acci�n
			TempData.Keep("CedulaIngresada");

			var viewModel = new CedulaScanViewModel();
			ViewBag.CedulaIngresada = TempData["CedulaIngresada"].ToString();

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ScanCedula(CedulaScanViewModel model)
		{
			// Verificar que tengamos la c�dula ingresada
			if (TempData["CedulaIngresada"] == null)
			{
				TempData["Error"] = "Error: Tienes que ingresar una c�dula antes de estar aqu�.";
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
				// Procesar imagen con OCR
				var resultado = await CedulaOCRService.ExtraerCedulaAsync(model.CedulaImageFile);

				// Verificar si la c�dula escaneada coincide con la ingresada
				if (resultado.Exitoso)
				{
					// Limpiar formato para comparar
					var cedulaEscaneadaLimpia = LimpiarCedula(resultado.NumeroCedula);
					var cedulaIngresadaLimpia = LimpiarCedula(cedulaIngresada);

					if (cedulaEscaneadaLimpia == cedulaIngresadaLimpia)
					{
						// �Coinciden! Redirigir a votaciones
						TempData.Remove("CedulaIngresada");
						return RedirectToAction("Votaciones", new { cedula = cedulaIngresada });
					}
					else
					{
						// No coinciden
						ViewBag.Error = $"La c�dula escaneada ({resultado.NumeroCedula}) no coincide con la c�dula ingresada ({cedulaIngresada}). Por favor, verifica que sea tu c�dula.";
						ViewBag.CedulaIngresada = cedulaIngresada;
						return View(model);
					}
				}
				else
				{
					// Error en el escaneo
					ViewBag.Error = $"No se pudo leer la c�dula correctamente: {resultado.ErrorMessage}";
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

		// NUEVA ACCI�N: Pantalla de resultados del OCR (mantenida para compatibilidad)
		[HttpGet]
		public IActionResult ResultadosOCR(bool exitoso, string numeroCedula = "",
			double confianza = 0, string textoCompleto = "", string errorMessage = "")
		{
			var viewModel = new ResultadosOCRViewModel
			{
				Exitoso = exitoso,
				NumeroCedula = numeroCedula,
				Confianza = confianza,
				TextoCompleto = textoCompleto,
				ErrorMessage = errorMessage
			};

			return View(viewModel);
		}

		// NUEVA ACCI�N: Confirmar y continuar con la c�dula extra�da (mantenida para compatibilidad)
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
				ModelState.AddModelError("CedulaConfirmada", "El formato de c�dula no es v�lido. Ejemplo: 001-0000000-1");
				return View("ResultadosOCR", model);
			}

			return RedirectToAction("Votaciones", new { cedula = model.CedulaConfirmada });
		}

		[HttpPost]
		public async Task<IActionResult> ProcessCedulaAjax(IFormFile cedulaFile)
		{
			try
			{
				if (cedulaFile == null || cedulaFile.Length == 0)
				{
					return Json(new { success = false, message = "No se recibi� ning�n archivo" });
				}

				var resultado = await CedulaOCRService.ExtraerCedulaAsync(cedulaFile);

				if (resultado.Exitoso)
				{
					return Json(new
					{
						success = true,
						cedula = resultado.NumeroCedula,
						confianza = resultado.Confianza.ToString("F1"),
						textoCompleto = resultado.TextoCompleto
					});
				}
				else
				{
					return Json(new
					{
						success = false,
						message = resultado.ErrorMessage,
						textoCompleto = resultado.TextoCompleto
					});
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
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

			// No todos los d�gitos iguales
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