using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Elecciones;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Elecciones;

namespace SADVO.Controllers
{
	public class EleccionesController : Controller
	{
		private readonly IEleccionesServices _eleccionesService;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;

		public EleccionesController(
			IEleccionesServices eleccionesService,
			IMapper mapper,
			IUserSession userSession)
		{
			_eleccionesService = eleccionesService;
			_mapper = mapper;
			_userSession = userSession;
		}

		private IActionResult CheckAuthorization()
		{
			if (!_userSession.hasUser())
				return RedirectToAction("Index", "Auth");
			if (!_userSession.checkRole())
				return RedirectToAction("AccessDenied", "Auth");
			return null!;
		}

		public async Task<IActionResult> Index()
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var elecciones = await _eleccionesService.GetAllEleccionesOrderedAsync();
			var viewModel = _mapper.Map<List<EleccionViewModel>>(elecciones);

			ViewBag.HayEleccionActiva = viewModel.Any(e => e.EsActiva);

			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			// Verificar que no hay elección activa
			var eleccionActiva = await _eleccionesService.GetEleccionActivaAsync();
			if (eleccionActiva != null)
			{
				TempData["Error"] = "Ya existe una elección activa. Debe finalizarla antes de crear una nueva.";
				return RedirectToAction("Index");
			}

			// Crear el modelo con fecha actual
			var model = new CreateEleccionViewModel
			{
				FechaRealizacion = DateTime.Now
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateEleccionViewModel model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			// Asegurar que la fecha no sea nula si no se especificó
			if (model.FechaRealizacion == default(DateTime))
			{
				model.FechaRealizacion = DateTime.Now;
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// Validar condiciones para crear elección
			var (isValid, errors) = await _eleccionesService.ValidarCreacionEleccionAsync();

			if (!isValid)
			{
				model.ErroresValidacion = errors;
				return View(model);
			}

			try
			{
				var dto = _mapper.Map<CrearEleccionDTO>(model);
				var result = await _eleccionesService.CreateEleccionWithValidationAsync(dto);

				if (result)
				{
					TempData["Success"] = $"La elección '{model.Nombre}' ha sido creada exitosamente.";
					return RedirectToAction("Index");
				}
				else
				{
					model.ErroresValidacion.Add("Error al crear la elección. Inténtelo nuevamente.");
					return View(model);
				}
			}
			catch (Exception ex)
			{
				model.ErroresValidacion.Add($"Error interno: {ex.Message}");
				return View(model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmarFinalizacion(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var eleccion = await _eleccionesService.GetByIdAsync(id);
			if (eleccion == null || !eleccion.EsActiva)
			{
				return NotFound();
			}

			var viewModel = new ConfirmarFinalizacionViewModel
			{
				EleccionId = eleccion.Id,
				EleccionNombre = eleccion.Nombre
			};

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> FinalizarEleccion(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var result = await _eleccionesService.FinalizarEleccionAsync(id);

			if (result)
			{
				TempData["Success"] = "La elección ha sido finalizada exitosamente.";
			}
			else
			{
				TempData["Error"] = "Error al finalizar la elección.";
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Resultados(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var eleccion = await _eleccionesService.GetByIdAsync(id);
			if (eleccion == null || eleccion.EsActiva)
			{
				return NotFound();
			}

			var resultados = await _eleccionesService.GetResultadosEleccionAsync(id);

			var viewModel = new ResultadosEleccionViewModel
			{
				EleccionId = eleccion.Id,
				EleccionNombre = eleccion.Nombre
			};

			// Agrupar resultados por puesto
			var resultadosPorPuesto = resultados.GroupBy(r => new { r.PuestoElectivoId, r.PuestoElectivoNombre })
				.Select(g => new ResultadoPorPuestoViewModel
				{
					PuestoNombre = g.Key.PuestoElectivoNombre,
					Candidatos = g.OrderByDescending(r => r.CantidadVotos)
							   .Select((r, index) => new ResultadoCandidatoViewModel
							   {
								   CandidatoNombre = r.CandidatoNombre,
								   PartidoPoliticoNombre = r.PartidoPoliticoNombre,
								   CantidadVotos = r.CantidadVotos,
								   Porcentaje = Math.Round(r.Porcentaje, 2),
								   EsGanador = index == 0
							   }).ToList()
				}).ToList();

			viewModel.ResultadosPorPuesto = resultadosPorPuesto;

			return View(viewModel);
		}
	}
}