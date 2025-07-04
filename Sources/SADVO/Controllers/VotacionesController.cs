﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Votacion;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Votacion;

namespace SADVO.Web.Controllers
{
	public class VotacionController : Controller
	{
		private readonly IVotacionService _votacionService;
		private readonly ICiudadanoSession _ciudadanoSession;
		private readonly IMapper _mapper;

		public VotacionController(
			IVotacionService votacionService,
			ICiudadanoSession ciudadanoSession,
			IMapper mapper)
		{
			_votacionService = votacionService;
			_ciudadanoSession = ciudadanoSession;
			_mapper = mapper;
		}

		// Método privado para validar ciudadano en sesión
		private bool ValidarCiudadanoEnSesion()
		{
			var ciudadano = _ciudadanoSession.GetCiudadanoSession();
			return ciudadano != null && ciudadano.Id > 0;
		}

		private IActionResult AccesoNoAutorizado()
		{
			TempData["Error"] = "Debe iniciar sesión como ciudadano para acceder a la votación.";
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			// Validar que hay un ciudadano en sesión
			if (!ValidarCiudadanoEnSesion())
				return AccesoNoAutorizado();

			var Ciudadano = _ciudadanoSession.GetCiudadanoSession();
			try
			{
				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(Ciudadano.Id);
				if (eleccionDto == null)
				{
					HttpContext.Session.Remove("Ciudadano");
					HttpContext.Session.Clear();
					TempData["Info"] = "No hay elecciones activas disponibles en este momento.";
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
			// Validar que hay un ciudadano en sesión
			if (!ValidarCiudadanoEnSesion())
				return AccesoNoAutorizado();

			var Ciudadano = _ciudadanoSession.GetCiudadanoSession();
			try
			{
				var puedeVotar = await _votacionService.PuedeVotarAsync(Ciudadano.Id);

				if (!puedeVotar)
				{
					TempData["Info"] = "Ya ha completado su votación en todos los puestos disponibles.";
					return RedirectToAction("Confirmacion");
				}

				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(Ciudadano.Id);
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
			// Validar que hay un ciudadano en sesión
			if (!ValidarCiudadanoEnSesion())
				return AccesoNoAutorizado();

			var Ciudadano = _ciudadanoSession.GetCiudadanoSession();
			try
			{
				var puedeVotar = await _votacionService.PuedeVotarAsync(Ciudadano.Id);
				if (!puedeVotar)
				{
					TempData["Info"] = "Ya ha completado su votación en todos los puestos disponibles.";
					return RedirectToAction("Confirmacion");
				}

				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(Ciudadano.Id);
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
			// Validar que hay un ciudadano en sesión
			if (!ValidarCiudadanoEnSesion())
				return AccesoNoAutorizado();

			var Ciudadano = _ciudadanoSession.GetCiudadanoSession();
			if (!ModelState.IsValid)
			{
				TempData["Error"] = "Los datos del formulario no son válidos.";
				return RedirectToAction("Votar");
			}

			try
			{
				var registrarVotoDto = _mapper.Map<RegistrarVotoDTO>(viewModel);
				// Asegurar que el CiudadanoId sea el de la sesión
				registrarVotoDto.CiudadanoId = Ciudadano.Id;

				var resultado = await _votacionService.RegistrarVotoAsync(registrarVotoDto);

				if (resultado.Exitoso)
				{
					TempData["Success"] = resultado.Mensaje ?? "Voto registrado exitosamente.";

					var puedeVotarMas = await _votacionService.PuedeVotarAsync(Ciudadano.Id);

					if (puedeVotarMas)
					{
						TempData["Info"] = "Voto registrado. Continúe votando en los puestos restantes.";
						return RedirectToAction("Votar");
					}
					else
					{
						// Limpiar sesión cuando complete toda la votación
						HttpContext.Session.Remove("User");
						HttpContext.Session.Clear();
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
			// Validar que hay un ciudadano en sesión
			if (!ValidarCiudadanoEnSesion())
				return AccesoNoAutorizado();

			var Ciudadano = _ciudadanoSession.GetCiudadanoSession();
			try
			{
				var eleccionDto = await _votacionService.GetEleccionParaVotarAsync(Ciudadano.Id);
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
					CiudadanoId = Ciudadano.Id,
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
			// Validar que hay un ciudadano en sesión
			if (!ValidarCiudadanoEnSesion())
				return AccesoNoAutorizado();

			var Ciudadano = _ciudadanoSession.GetCiudadanoSession();
			if (!ModelState.IsValid)
			{
				TempData["Error"] = "Los datos de confirmación no son válidos.";
				return RedirectToAction("Votar");
			}

			// Asegurar que el CiudadanoId sea el de la sesión
			model.CiudadanoId = Ciudadano.Id;

			try
			{
				var mappedEntity = _mapper.Map<RegistrarVotoDTO>(model);
				var resultado = await _votacionService.RegistrarVotoAsync(mappedEntity);

				if (resultado.Exitoso)
				{
					TempData["Success"] = resultado.Mensaje;

					var puedeVotarMas = await _votacionService.PuedeVotarAsync(Ciudadano.Id);

					if (puedeVotarMas)
					{
						return RedirectToAction("Votar");
					}
					else
					{
						// Limpiar sesión cuando complete toda la votación
						HttpContext.Session.Remove("User");
						HttpContext.Session.Clear();
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
			HttpContext.Session.Remove("Ciudadano");
			HttpContext.Session.Clear();
			return View();
		}

		[HttpGet]
		public IActionResult NoEleccionesActivas()
		{
			HttpContext.Session.Clear();
			if (!ValidarCiudadanoEnSesion())
				return AccesoNoAutorizado();

			return View();
		}

		[HttpGet]
		public IActionResult Error()
		{
			return View();
		}
	}
}