using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.Services;
using SADVO.Core.Application.ViewModels.Dirigente;

namespace SADVO.Controllers
{
	public class DirigenteController : Controller
	{
		private readonly IUserSession _userSession;
		private readonly IAsignacionDirigentesServices _asignacionDirigenteServices;
		// Agregar estos servicios para obtener datos del dashboard
		private readonly ICandidatoServices _candidatosServices;
		private readonly IAsignacionCandidatosServices _asignacionCandidatosServices;
		private readonly IPuestosElectivosServices _puestosElectivosServices;
		private readonly IPartidoPoliticoServices _partidoPoliticoServices;

		public DirigenteController(
			IUserSession userSession,
			IAsignacionDirigentesServices asignacionDirigentesServices,
			ICandidatoServices candidatosServices,
			IAsignacionCandidatosServices asignacionCandidatosServices,
			IPartidoPoliticoServices partidoPoliticoServices,
			IPuestosElectivosServices puestosElectivosServices)
		{
			_userSession = userSession;
			_asignacionDirigenteServices = asignacionDirigentesServices;
			_candidatosServices = candidatosServices;
			_asignacionCandidatosServices = asignacionCandidatosServices;
			_puestosElectivosServices = puestosElectivosServices;
			_partidoPoliticoServices = partidoPoliticoServices;
		}

		private async Task<IActionResult?> ValidateUserAndPartido()
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}
			if (_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AccessDenied" });
			}
			var currentSession = _userSession.GetUserSession();
			var userId = currentSession!.Id;
			try
			{
				await _asignacionDirigenteServices.GetDirigente(userId);
				return null;
			}
			catch (InvalidOperationException)
			{
				return RedirectToRoute(new { controller = "Auth", action = "NoPartidoPolitico" });
			}
		}

		public async Task<IActionResult> Index()
		{
			var validationResult = await ValidateUserAndPartido();
			if (validationResult != null)
			{
				return validationResult;
			}

			try
			{
				var currentSession = _userSession.GetUserSession();
				var userId = currentSession!.Id;

				var dirigente = await _asignacionDirigenteServices.GetDirigente(userId);
				var partido = await _partidoPoliticoServices.GetByIdAsync(dirigente.PartidoPoliticoId);

				var dashboardViewModel = new DashboardDirigenteViewModel
				{
					PartidoPoliticoNombre = partido!.Nombre,
					PartidoPoliticoSiglas = partido.Siglas,
					PartidoPoliticoId = dirigente.PartidoPoliticoId
				};

				// Obtener estadísticas del partido
				var candidatos = await _candidatosServices.GetCandidatosByPartidoAsync(dirigente.PartidoPoliticoId);
				dashboardViewModel.TotalCandidatos = candidatos.Count;

				var asignaciones = await _asignacionCandidatosServices.GetAsignacionesByPartidoAsync(dirigente.PartidoPoliticoId);
				dashboardViewModel.CandidatosAsignados = asignaciones.Count;

				var puestosDisponibles = await _puestosElectivosServices.GetAllAsync();
				dashboardViewModel.TotalPuestosDisponibles = puestosDisponibles.Count;
				return View(dashboardViewModel);
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al cargar el dashboard. Por favor, intente nuevamente.";
				return View(new DashboardDirigenteViewModel());
			}
		}
	}
}