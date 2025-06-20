using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Alianzas;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Alianzas;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;

namespace SADVO.Controllers
{
	public class AlianzasPoliticasController : Controller
	{
		private readonly IAlianzasPoliticasServices _alianzasService;
		private readonly IAsignacionDirigentesServices _asignacionDirigentesService;
		private readonly IAlianzasHelperService _alianzasHelperService;
		private readonly IEleccionesServices _eleccionesServices;
		private readonly IUserSession _userSession;
		private readonly IMapper _mapper;

		public AlianzasPoliticasController(
			IAlianzasPoliticasServices alianzasService,
			IAsignacionDirigentesServices asignacionDirigentesService,
			IUserSession userSession,
			IAlianzasHelperService alianzasHelperService,
			IEleccionesServices eleccionesServices,
			IMapper mapper)
		{
			_alianzasService = alianzasService;
			_asignacionDirigentesService = asignacionDirigentesService;
			_userSession = userSession;
			_alianzasHelperService = alianzasHelperService;
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

		// GET: Listado principal de alianzas
		public async Task<IActionResult> Index()
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var user = _userSession.GetUserSession();
			var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);

			if (partidoId == null)
				return RedirectToAction("Index", "Home");

			var pendientes = await _alianzasService.GetSolicitudesPendientesAsync(partidoId.Value);
			var enviadas = await _alianzasService.GetSolicitudesEnviadasAsync(partidoId.Value);
			var activas = await _alianzasService.GetAlianzasActivasAsync(partidoId.Value);

			var model = new AlianzasViewModel
			{
				SolicitudesPendientes = pendientes,
				SolicitudesEnviadas = enviadas,
				AlianzasActivas = activas
			};

			return View(model);
		}

		// GET: Formulario para crear nueva alianza
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var user = _userSession.GetUserSession();
			var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);

			if (partidoId == null)
				return RedirectToAction("Index", "Home");

			var partidosDisponibles = await _alianzasHelperService.GetPartidosDisponiblesParaAlianzaAsync(partidoId.Value);

			var model = new CrearAlianzaViewModel
			{
				PartidoSolicitanteId = partidoId.Value,
				PartidosDisponibles = _mapper.Map<List<PartidosPoliticosViewModel>>(partidosDisponibles)
			};

			return View(model);
		}

		// POST: Crear nueva alianza
		[HttpPost]
		public async Task<IActionResult> Create(CrearAlianzaViewModel model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
			{
				var user = _userSession.GetUserSession();
				var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);
				if (partidoId != null)
				{
					var partidosDisponibles = await _alianzasHelperService.GetPartidosDisponiblesParaAlianzaAsync(partidoId.Value);
					model.PartidosDisponibles = _mapper.Map<List<PartidosPoliticosViewModel>>(partidosDisponibles);
				}
				return View(model);
			}

			var dto = _mapper.Map<CrearAlianzaDTO>(model);
			var resultado = await _alianzasService.AddAsync(dto);

			if (!resultado)
			{
				ModelState.AddModelError("", "No se pudo crear la solicitud. Verifique que no exista una relación pendiente con este partido.");

				var user = _userSession.GetUserSession();
				var partidoId = await _asignacionDirigentesService.GetPartidoIdByUserIdAsync(user.Id);
				if (partidoId != null)
				{
					var partidosDisponibles = await _alianzasHelperService.GetPartidosDisponiblesParaAlianzaAsync(partidoId.Value);
					model.PartidosDisponibles = _mapper.Map<List<PartidosPoliticosViewModel>>(partidosDisponibles);
				}
				return View(model);
			}

			TempData["Success"] = "Solicitud de alianza enviada correctamente.";
			return RedirectToAction("Index");
		}

		// GET: Confirmar aceptación de alianza
		[HttpGet]
		public async Task<IActionResult> ConfirmarAceptar(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var alianza = await _alianzasService.GetByIdAsync(id);
			if (alianza == null) return NotFound();

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var model = new ConfirmacionViewModel
			{
				Id = id,
				Mensaje = $"¿Está seguro que desea aceptar la alianza con el partido {alianza.NombrePartidoConSiglas(true)}?",
				Accion = "AceptarConfirmado",
				TituloBoton = "Aceptar",
			};

			return View("Confirmacion", model);
		}

		// POST: Aceptar alianza confirmada
		[HttpPost]
		public async Task<IActionResult> AceptarConfirmado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var resultado = await _alianzasService.AceptarSolicitudAsync(id);

			if (resultado)
				TempData["Success"] = "Alianza aceptada correctamente.";
			else
				TempData["Error"] = "No se pudo aceptar la alianza.";

			return RedirectToAction("Index");
		}

		// GET: Confirmar rechazo de alianza
		[HttpGet]
		public async Task<IActionResult> ConfirmarRechazar(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var alianza = await _alianzasService.GetByIdAsync(id);
			if (alianza == null) return NotFound();

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var model = new ConfirmacionViewModel
			{
				Id = id,
				Mensaje = $"¿Está seguro que desea rechazar la alianza con el partido {alianza.NombrePartidoConSiglas(true)}?",
				Accion = "RechazarConfirmado",
				TituloBoton = "Rechazar",
			};

			return View("Confirmacion", model);
		}

		// POST: Rechazar alianza confirmada
		[HttpPost]
		public async Task<IActionResult> RechazarConfirmado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var resultado = await _alianzasService.RechazarSolicitudAsync(id);

			if (resultado)
				TempData["Success"] = "Alianza rechazada correctamente.";
			else
				TempData["Error"] = "No se pudo rechazar la alianza.";

			return RedirectToAction("Index");
		}

		// GET: Confirmar eliminación de solicitud
		[HttpGet]
		public async Task<IActionResult> ConfirmarEliminar(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var alianza = await _alianzasService.GetByIdAsync(id);
			if (alianza == null) return NotFound();

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var model = new ConfirmacionViewModel
			{
				Id = id,
				Mensaje = $"¿Está seguro que desea eliminar la solicitud de alianza con el partido {alianza.NombrePartidoConSiglas(false)}?",
				Accion = "EliminarConfirmado",
				TituloBoton = "Eliminar",
			};

			return View("Confirmacion", model);
		}

		// POST: Eliminar solicitud confirmada
		[HttpPost]
		public async Task<IActionResult> EliminarConfirmado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var resultado = await _alianzasService.EliminarSolicitudAsync(id);

			if (resultado)
				TempData["Success"] = "Solicitud eliminada correctamente.";
			else
				TempData["Error"] = "No se pudo eliminar la solicitud.";

			return RedirectToAction("Index");
		}

		// GET: Confirmar romper alianza
		[HttpGet]
		public async Task<IActionResult> ConfirmarRomper(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			var alianza = await _alianzasService.GetByIdAsync(id);
			if (alianza == null) return NotFound();

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var model = new ConfirmacionViewModel
			{
				Id = id,
				Mensaje = $"¿Está seguro que desea romper la alianza con el partido {alianza.NombrePartidoConSiglas(true)}?",
				Accion = "RomperConfirmado",
				TituloBoton = "Romper Alianza",
			};

			return View("Confirmacion", model);
		}

		// POST: Romper alianza confirmada
		[HttpPost]
		public async Task<IActionResult> RomperConfirmado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			try
			{
				var resultado = await _alianzasHelperService.RomperAlianzaAsync(id);

				if (resultado)
					TempData["Success"] = "Alianza rota correctamente.";
				else
					TempData["Error"] = "No se pudo romper la alianza.";
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"Error al romper la alianza: {ex.Message}";
			}

			return RedirectToAction("Index");
		}
	}
}
