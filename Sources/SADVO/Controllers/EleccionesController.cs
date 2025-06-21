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

			var eleccionActiva = await _eleccionesService.GetEleccionActivaAsync();
			if (eleccionActiva != null)
			{
				TempData["Error"] = "Ya existe una elección activa. Debe finalizarla antes de crear una nueva.";
				return RedirectToAction("Index");
			}

			var model = new CreateEleccionViewModel
			{
				Año = DateTime.Now.Year 
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateEleccionViewModel model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var dto = _mapper.Map<CrearEleccionDTO>(model);
				var (success, errors) = await _eleccionesService.CreateEleccionWithValidationAsync(dto);

				if (success)
				{
					TempData["Success"] = $"La elección '{model.Nombre}' ha sido creada exitosamente.";
					return RedirectToAction("Index");
				}
				else
				{
					model.ErroresValidacion.AddRange(errors);
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
			try
			{
				var authResult = CheckAuthorization();
				if (authResult != null) return authResult;

				var eleccion = await _eleccionesService.GetByIdAsync(id);
				if (eleccion == null)
				{
					TempData["Error"] = "La elección solicitada no existe.";
					return RedirectToAction("Index");
				}

				if (eleccion.EsActiva)
				{
					TempData["Error"] = "No se pueden ver los resultados de una elección activa.";
					return RedirectToAction("Index");
				}

				var resultados = await _eleccionesService.GetResultadosEleccionAsync(id);
				if (resultados == null || !resultados.Any())
				{
					TempData["Warning"] = "Esta elección no tiene votos registrados aún.";
				}

				var viewModel = new ResultadosEleccionViewModel
				{
					EleccionId = eleccion.Id,
					EleccionNombre = eleccion.Nombre,
					EleccionAnio = eleccion.Año,
					FechaFinalizacion = eleccion.FechaFinalizacion
				};

				if (resultados != null && resultados.Any())
				{
					var resultadosPorPuesto = resultados.GroupBy(r => new { r.PuestoElectivoId, r.PuestoElectivoNombre })
						.Select(g => new ResultadoPorPuestoViewModel
						{
							PuestoElectivoId = g.Key.PuestoElectivoId,
							PuestoNombre = g.Key.PuestoElectivoNombre,
							TotalVotos = g.Sum(r => r.CantidadVotos),
							Candidatos = g.OrderByDescending(r => r.CantidadVotos)
									   .ThenBy(r => r.CandidatoNombre) 
									   .Select((r, index) => new ResultadoCandidatoViewModel
									   {
										   CandidatoId = r.CandidatoId,
										   CandidatoNombre = r.CandidatoNombre,
										   PartidoPoliticoNombre = r.PartidoPoliticoNombre,
										   CantidadVotos = r.CantidadVotos,
										   Porcentaje = Math.Round(r.Porcentaje, 2),
										   EsGanador = index == 0 && r.CantidadVotos > 0, 
										   Posicion = index + 1
									   }).ToList()
						})
						.OrderBy(p => p.PuestoNombre) 
						.ToList();

					viewModel.ResultadosPorPuesto = resultadosPorPuesto;

					viewModel.TotalCandidatos = resultados.Count;
					viewModel.TotalVotosEmitidos = resultados.Sum(r => r.CantidadVotos);
					viewModel.TotalPuestosDisputados = resultadosPorPuesto.Count;
				}
				else
				{
					viewModel.ResultadosPorPuesto = new List<ResultadoPorPuestoViewModel>();
					viewModel.TotalCandidatos = 0;
					viewModel.TotalVotosEmitidos = 0;
					viewModel.TotalPuestosDisputados = 0;
				}

				return View(viewModel);
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Ocurrió un error al cargar los resultados de la elección. Por favor, intente nuevamente.";
				return RedirectToAction("Index");
			}
		}
	}
}