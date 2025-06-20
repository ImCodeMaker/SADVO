using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Votacion;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Votacion;
using System.Security.Claims;

namespace SADVO.Web.Controllers
{
	public class VotacionController : Controller
	{
		private readonly IVotacionService _votacionService;
		private readonly IUserSession _userSession;
		private readonly IMapper _mapper;

		public VotacionController(
			IVotacionService votacionService,
			IUserSession userSession,
			IMapper mapper)
		{
			_votacionService = votacionService;
			_userSession = userSession;
			_mapper = mapper;
		}

		private int GetCurrentUserId()
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return int.TryParse(userIdClaim, out int userId) ? userId : 0;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			try
			{
				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(1);
				if (eleccionDto == null)
				{
					TempData["Info"] = "No hay elecciones activas disponibles en este momento.";
					return View("NoEleccionesActivas");
				}

				var eleccionViewModel = _mapper.Map<EleccionVotacionViewModel>(eleccionDto);
				return View(eleccionViewModel);
			}
			catch (Exception)
			{
				TempData["Error"] = "Ocurrió un error al cargar la información de votación. Por favor, intente nuevamente.";
				return View("Error");
			}
		}

		[HttpGet]
		public async Task<IActionResult> VerificarEstado()
		{
			try
			{
				var puedeVotar = await _votacionService.PuedeVotarAsync(1);

				if (!puedeVotar)
				{
					TempData["Info"] = "Ya ha completado su votación en todos los puestos disponibles.";
					return RedirectToAction("Confirmacion");
				}

				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(1);
				if (eleccionDto == null)
				{
					TempData["Info"] = "No hay elecciones activas disponibles.";
					return View("NoEleccionesActivas");
				}

				var eleccionViewModel = _mapper.Map<EleccionVotacionViewModel>(eleccionDto);
				return View(eleccionViewModel);
			}
			catch (Exception)
			{
				TempData["Error"] = "Error al verificar estado de votación.";
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Votar()
		{
			try
			{
				var puedeVotar = await _votacionService.PuedeVotarAsync(1);
				if (!puedeVotar)
				{
					TempData["Info"] = "Ya ha completado su votación en todos los puestos disponibles.";
					return RedirectToAction("Confirmacion");
				}

				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(1);
				if (eleccionDto == null)
				{
					TempData["Info"] = "No hay elecciones activas disponibles.";
					return View("NoEleccionesActivas");
				}

				var eleccionViewModel = _mapper.Map<EleccionVotacionViewModel>(eleccionDto);
				return View(eleccionViewModel);
			}
			catch (Exception)
			{
				TempData["Error"] = "Ocurrió un error al cargar la información de votación.";
				return View("Error");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegistrarVoto(RegistrarVotoViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				TempData["Error"] = "Los datos del formulario no son válidos.";
				return RedirectToAction("Votar");
			}

			try
			{
				var registrarVotoDto = _mapper.Map<RegistrarVotoDTO>(viewModel);
				var resultado = await _votacionService.RegistrarVotoAsync(registrarVotoDto);

				if (resultado.Exitoso)
				{
					TempData["Success"] = resultado.Mensaje ?? "Voto registrado exitosamente.";

					// CAMBIO CLAVE: Verificar si completó toda la votación
					var puedeVotarMas = await _votacionService.PuedeVotarAsync(1);

					if (puedeVotarMas)
					{
						// Aún puede votar en otros puestos, redirigir de vuelta a votar
						TempData["Info"] = "Voto registrado. Continúe votando en los puestos restantes.";
						return RedirectToAction("Votar");
					}
					else
					{
						// Ya completó toda la votación
						return RedirectToAction("Confirmacion");
					}
				}
				else
				{
					TempData["Error"] = string.Join("; ", resultado.Errores);
					return RedirectToAction("Votar");
				}
			}
			catch (Exception)
			{
				TempData["Error"] = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
				return RedirectToAction("Votar");
			}
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmarVoto(int eleccionId, int puestoElectivoId, int candidatoId)
		{
			try
			{
				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(1); // Usar ciudadanoId = 1 para testing
				if (eleccionDto == null)
				{
					TempData["Error"] = "No hay elecciones activas disponibles.";
					return RedirectToAction("Index");
				}

				var puesto = eleccionDto.PuestosElectivos.FirstOrDefault(p => p.PuestoElectivoId == puestoElectivoId);
				if (puesto == null)
				{
					TempData["Error"] = "Puesto electivo no encontrado.";
					return RedirectToAction("Votar");
				}

				// Verificar si ya votó en este puesto
				if (puesto.YaVotado)
				{
					TempData["Error"] = "Ya ha votado en este puesto electivo.";
					return RedirectToAction("Votar");
				}

				var candidato = puesto.Candidatos.FirstOrDefault(c => c.CandidatoId == candidatoId);
				if (candidato == null)
				{
					TempData["Error"] = "Candidato no encontrado.";
					return RedirectToAction("Votar");
				}

				var model = new RegistrarVotoViewModel
				{
					EleccionId = eleccionId,
					PuestoElectivoId = puestoElectivoId,
					CandidatoId = candidatoId,
					CiudadanoId = 1, // Hardcodeado para testing
					PartidoPoliticoId = candidato.PartidoPrincipalId
				};

				ViewBag.NombreEleccion = eleccionDto.NombreEleccion;
				ViewBag.NombrePuesto = puesto.NombrePuesto;
				ViewBag.NombreCandidato = $"{candidato.NombreCandidato} {candidato.ApellidoCandidato}";
				ViewBag.PartidoCandidato = candidato.NombrePartidoPrincipal;

				return View(model);
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al cargar la confirmación de voto.";
				return RedirectToAction("Votar");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ConfirmarVoto(RegistrarVotoViewModel model)
		{
			if (!ModelState.IsValid)
			{
				TempData["Error"] = "Los datos de confirmación no son válidos.";
				return RedirectToAction("Votar");
			}

			model.CiudadanoId = 1; // Asegurar que el CiudadanoId esté seteado

			try
			{
				var mappedEntity = _mapper.Map<RegistrarVotoDTO>(model);
				var resultado = await _votacionService.RegistrarVotoAsync(mappedEntity);

				if (resultado.Exitoso)
				{
					TempData["Success"] = resultado.Mensaje;

					// CAMBIO CLAVE: Verificar si puede seguir votando
					var puedeVotarMas = await _votacionService.PuedeVotarAsync(1);

					if (puedeVotarMas)
					{
						// Continuar votando
						return RedirectToAction("Votar");
					}
					else
					{
						// Votación completa
						return RedirectToAction("Confirmacion");
					}
				}
				else
				{
					TempData["Error"] = string.Join("; ", resultado.Errores);
					return RedirectToAction("Votar");
				}
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"Ocurrió un error al registrar su voto: {ex.Message}";
				return RedirectToAction("Votar");
			}
		}

		[HttpGet]
		public IActionResult Confirmacion()
		{
			return View();
		}

		[HttpGet]
		public IActionResult NoEleccionesActivas()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Error()
		{
			return View();
		}
	}
}