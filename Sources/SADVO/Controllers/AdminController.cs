using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SADVO.Core.Application.Interfaces;
using AutoMapper;
using SADVO.Core.Application.ViewModels.ResumenElectoral;

namespace SADVO.Controllers
{
	public class AdminController : Controller
	{
		private readonly IUserSession _userSession;
		private readonly IEleccionesServices _eleccionesServices;
		private readonly IMapper _mapper;

		public AdminController(
			IUserSession userSession,
			IEleccionesServices eleccionesServices,
			IMapper mapper)
		{
			_userSession = userSession;
			_eleccionesServices = eleccionesServices;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			if (!_userSession.hasUser())
				return RedirectToRoute(new { controller = "Auth", action = "Index" });

			if (!_userSession.checkRole())
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });

			var viewModel = new ResumenElectoralViewModel();

			try
			{
				var aniosDisponibles = await _eleccionesServices.GetAniosDisponiblesAsync();

				if (aniosDisponibles.Años.Any())
				{
					viewModel.AñosDisponibles = aniosDisponibles.Años;
					viewModel.AñoSeleccionado = aniosDisponibles.AñoMasReciente;

					// Pasar SelectList por ViewBag
					ViewBag.AniosSelectList = aniosDisponibles.Años
						.Select(a => new SelectListItem
						{
							Value = a.ToString(),
							Text = a.ToString(),
							Selected = a == aniosDisponibles.AñoMasReciente
						})
						.ToList();
				}
				else
				{
					ViewBag.Message = "No hay elecciones finalizadas disponibles para mostrar el resumen electoral.";
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = "Error al cargar los datos: " + ex.Message;
			}

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ObtenerResumenElectoral(ResumenElectoralViewModel model)
		{
			if (!_userSession.hasUser())
				return RedirectToRoute(new { controller = "Auth", action = "Index" });

			if (!_userSession.checkRole())
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });

			try
			{
				var aniosDisponibles = await _eleccionesServices.GetAniosDisponiblesAsync();
				model.AñosDisponibles = aniosDisponibles.Años;

				ViewBag.AniosSelectList = aniosDisponibles.Años
					.Select(a => new SelectListItem
					{
						Value = a.ToString(),
						Text = a.ToString(),
						Selected = a == model.AñoSeleccionado
					})
					.ToList();

				if (ModelState.IsValid)
				{
					var resumen = await _eleccionesServices.GetResumenElectoralPorAnioAsync(model.AñoSeleccionado);
					model.Elecciones = _mapper.Map<List<EleccionResumenViewModel>>(resumen);
					model.MostrarResultados = true;

					if (!model.Elecciones.Any())
						ViewBag.Message = $"No se encontraron elecciones finalizadas para el año {model.AñoSeleccionado}.";
				}
				else
				{
					ViewBag.Error = "Por favor, seleccione un año válido.";
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = "Error al obtener el resumen electoral: " + ex.Message;
				model.MostrarResultados = false;
			}

			return View("Index", model);
		}
	}
}
