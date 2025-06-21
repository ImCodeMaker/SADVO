using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Ciudadanos;

namespace SADVO.Controllers
{
	public class CiudadanosController : Controller
	{
		private readonly ICiudadanosServices _ciudadanosService;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;
		private readonly IEleccionesServices _eleccionesServices;

		public CiudadanosController(
			ICiudadanosServices ciudadanosService,
			IMapper mapper,
			IUserSession userSession,
			IEleccionesServices eleccionesServices)
		{
			_ciudadanosService = ciudadanosService;
			_mapper = mapper;
			_userSession = userSession;
			_eleccionesServices = eleccionesServices;
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

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var ciudadanos = await _ciudadanosService.GetAllAsync();
			return View(_mapper.Map<List<CiudadanosViewModel>>(ciudadanos));
		}

		public async Task<IActionResult> Create()
		{
			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var authResult = CheckAuthorization();
			return authResult ?? View();
		}

		public async Task<IActionResult> Update(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var ciudadano = await _ciudadanosService.GetByIdAsync(id);
			if (ciudadano == null) return NotFound();

			return View(_mapper.Map<UpdateCiudadanosViewModel>(ciudadano));
		}

		[HttpPost]
		public async Task<IActionResult> Create(CrearCiudadanosViewModel model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
				return View(model);

			try
			{
				var createDto = _mapper.Map<CrearCiudadanos>(model);
				await _ciudadanosService.AddAsync(createDto);

				return RedirectToAction("Index");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, UpdateCiudadanosViewModel model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
				return View("Update", model);

			try
			{
				var updateDto = _mapper.Map<UpdateCiudadanosDTO>(model);
				await _ciudadanosService.UpdateAsync(id, updateDto);

				return RedirectToAction("Index");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View("Update", model);
			}
		}

		public async Task<IActionResult> ChangeStatus(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var ciudadano = await _ciudadanosService.GetByIdAsync(id);
			if (ciudadano == null) return NotFound();

			return View("ConfirmarEstado", _mapper.Map<CiudadanosViewModel>(ciudadano));
		}

		[HttpPost]
		public async Task<IActionResult> ToggleEstado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var ciudadano = await _ciudadanosService.GetByIdAsync(id);
			if (ciudadano == null) return NotFound();

			await _ciudadanosService.UpdateEstadoAsync(id, !ciudadano.Estado);
			return RedirectToAction("Index");
		}
	}
}