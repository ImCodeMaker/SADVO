using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.PuestosElectivos;

namespace SADVO.Controllers
{
	public class PuestosElectivosController : Controller
	{
		private readonly IPuestosElectivosServices _puestosService;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;
		private readonly IEleccionesServices _eleccionesServices;

		public PuestosElectivosController(
			IPuestosElectivosServices puestosService,
			IMapper mapper,
			IUserSession userSession,
			IEleccionesServices eleccionesServices)
		{
			_puestosService = puestosService;
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

			var puestos = await _puestosService.GetAllAsync();
			return View(_mapper.Map<List<PuestoElectivoViewModel>>(puestos));
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

			var puesto = await _puestosService.GetByIdAsync(id);
			if (puesto == null) return NotFound();

			return View(_mapper.Map<UpdatePuestoElectivoViewModel>(puesto));
		}

		[HttpPost]
		public async Task<IActionResult> Create(CrearPuestoElectivoDTO model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (!ModelState.IsValid)
				return View(model);

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			try
			{
				var createDto = _mapper.Map<CrearPuestoElectivoDTO>(model);
				await _puestosService.AddAsync(createDto);

				return RedirectToAction("Index");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError(nameof(model.Nombre), ex.Message);
				return View(model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, UpdatePuestoElectivoDTO model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (!ModelState.IsValid)
				return View("Update", model);

			if (await _eleccionesServices.HayEleccionActivaAsync())
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			try
			{
				var updateDto = _mapper.Map<UpdatePuestoElectivoDTO>(model);
				await _puestosService.UpdateAsync(id, updateDto);

				return RedirectToAction("Index");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError(nameof(model.Nombre), ex.Message);
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

			var puesto = await _puestosService.GetByIdAsync(id);
			if (puesto == null) return NotFound();

			return View("ConfirmarEstado", _mapper.Map<PuestoElectivoViewModel>(puesto));
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

			var puesto = await _puestosService.GetByIdAsync(id);
			if (puesto == null) return NotFound();

			await _puestosService.UpdateEstadoAsync(id, !puesto.Estado);
			return RedirectToAction("Index");
		}
	}
}