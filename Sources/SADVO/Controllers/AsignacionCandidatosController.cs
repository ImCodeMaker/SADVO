using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.AsignacionCandidatos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.AsignacionCandidatos;
using SADVO.Core.Application.ViewModels.Candidatos;
using SADVO.Core.Application.ViewModels.PuestosElectivos;

namespace SADVO.Controllers
{
	public class AsignacionCandidatosController : Controller
	{
		private readonly IAsignacionCandidatosServices _asignacionServices;
		private readonly IAsignacionCandidatosHelperService _helperService;
		private readonly IAsignacionDirigentesServices _asignacionDirigentesService;
		private readonly IEleccionesServices _eleccionesServices;
		private readonly IUserSession _userSession;
		private readonly IMapper _mapper;

		public AsignacionCandidatosController(
			IAsignacionCandidatosServices asignacionServices,
			IAsignacionCandidatosHelperService helperService,
			IAsignacionDirigentesServices asignacionDirigentesService,
			IEleccionesServices eleccionesServices,
			IUserSession userSession,
			IMapper mapper)
		{
			_asignacionServices = asignacionServices;
			_helperService = helperService;
			_asignacionDirigentesService = asignacionDirigentesService;
			_userSession = userSession;
			_eleccionesServices = eleccionesServices;
			_mapper = mapper;
		}

		private IActionResult CheckAuthorization()
		{
			if (!_userSession.hasUser())
				return RedirectToAction("Index", "Auth");
			if (_userSession.checkRole())
				return RedirectToAction("AccessDenied", "Auth");
			return null!;
		}

		public async Task<IActionResult> Index()
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var user = _userSession.GetUserSession();
			var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);
			if (partidoId == null)
				return RedirectToAction("Index", "Home");

			// Obtener TODAS las asignaciones (propias + respaldos)
			var todasAsignaciones = await _helperService.GetAsignacionesConAliadosAsync(partidoId.Value);

			// Debug info
			Console.WriteLine($"Total de asignaciones: {todasAsignaciones.Count}");

			// Filtrar para obtener SOLO las asignaciones ORIGINALES (no respaldos)
			var asignacionesOriginales = todasAsignaciones
				.Where(a => a.PartidoRespaldaId == null)
				.ToList();

			// Para cada asignación original, obtener sus respaldos
			var asignacionesAgrupadas = new List<AsignacionCandidatoViewModel>();

			foreach (var original in asignacionesOriginales)
			{
				var viewModel = _mapper.Map<AsignacionCandidatoViewModel>(original);

				// Buscar respaldos para esta asignación original
				var respaldos = todasAsignaciones
					.Where(a => a.CandidatoId == original.CandidatoId &&
							   a.PuestoElectivoId == original.PuestoElectivoId &&
							   a.PartidoRespaldaId != null &&
							   !string.IsNullOrEmpty(a.PartidoRespaldanteConSiglas))
					.Select(a => a.PartidoRespaldanteConSiglas!)
					.Distinct()
					.ToList();

				viewModel.PartidosQueRespaldan = respaldos;

				// Debug info
				Console.WriteLine($"{original.NombreCompleto} - ID: {original.CandidatoId} - Respaldos: {respaldos.Count}");

				asignacionesAgrupadas.Add(viewModel);
			}

			var model = new AsignacionCandidatosViewModel
			{
				Asignaciones = asignacionesAgrupadas,
				PuedeAgregar = true
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var user = _userSession.GetUserSession();
			var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);

			if (partidoId == null)
				return RedirectToAction("Index", "Home");

			var candidatosDisponibles = await _helperService.GetCandidatosDisponiblesAsync(partidoId.Value);
			var puestosDisponibles = await _helperService.GetPuestosDisponiblesAsync(partidoId.Value);

			var model = new CrearAsignacionCandidatoViewModel
			{
				PartidoPoliticoId = partidoId.Value,
				CandidatosDisponibles = _mapper.Map<List<CandidatosViewModel>>(candidatosDisponibles),
				PuestosDisponibles = _mapper.Map<List<PuestoElectivoViewModel>>(puestosDisponibles),
				EsRespaldo = false,
				PartidoRespaldaId = null
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CrearAsignacionCandidatoViewModel model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
			{
				var user = _userSession.GetUserSession();
				var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);
				if (partidoId != null)
				{
					model.CandidatosDisponibles = _mapper.Map<List<CandidatosViewModel>>(
						await _helperService.GetCandidatosDisponiblesAsync(partidoId.Value));

					// Aquí pasamos también model.CandidatoId para filtrar puestos correctamente
					model.PuestosDisponibles = _mapper.Map<List<PuestoElectivoViewModel>>(
						await _helperService.GetPuestosDisponiblesAsync(partidoId.Value, model.CandidatoId));
				}
				return View(model);
			}

			try
			{
				var user = _userSession.GetUserSession();
				var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);

				if (partidoId == null)
				{
					TempData["Error"] = "No se pudo determinar el partido político.";
					return RedirectToAction("Index");
				}

				var candidatoPerteneceAPartidoAliado = await _helperService.CandidatoPerteneceAPartidoAliado(
					model.CandidatoId, partidoId.Value);

				model.EsRespaldo = candidatoPerteneceAPartidoAliado;
				if (candidatoPerteneceAPartidoAliado)
				{
					model.PartidoRespaldaId = partidoId.Value;
				}
				else
				{
					model.PartidoRespaldaId = null;
				}

				var dto = _mapper.Map<CrearAsignacionCandidatoDTO>(model);
				var resultado = await _asignacionServices.AddAsync(dto);

				if (!resultado)
				{
					TempData["Error"] = "No se pudo crear la asignación. Verifique las reglas de asignación.";
					return RedirectToAction("Create");
				}

				TempData["Success"] = "Asignación creada correctamente.";
				return RedirectToAction("Index");
			}
			catch (InvalidOperationException ex)
			{
				TempData["Error"] = ex.Message;
				return RedirectToAction("Create");
			}
			catch (Exception)
			{
				TempData["Error"] = "Ocurrió un error al procesar la solicitud.";
				return RedirectToAction("Index");
			}
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmarEliminar(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var asignacion = await _asignacionServices.GetByIdWithIncludesAsync(id);
			if (asignacion == null) return NotFound();

			var model = new ConfirmarEliminarAsignacionViewModel
			{
				Id = id,
				NombreCandidato = asignacion.NombreCandidato,
				ApellidoCandidato = asignacion.ApellidoCandidato,
				NombrePuestoElectivo = asignacion.NombrePuestoElectivo
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EliminarConfirmado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			try
			{
				await _asignacionServices.UpdateEstadoAsync(id, false);
				TempData["Success"] = "Asignación eliminada correctamente.";
			}
			catch (Exception)
			{
				TempData["Error"] = "No se pudo eliminar la asignación.";
			}

			return RedirectToAction("Index");
		}
	}
}