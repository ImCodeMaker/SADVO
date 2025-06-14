using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.PuestosElectivos;


namespace SADVO.Controllers
{
	public class PuestosElectivosController : Controller
	{
		private readonly IPuestosElectivosServices _puestosElectivosServices;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;

		public PuestosElectivosController(IPuestosElectivosServices puestosElectivosServices, IMapper mapper, IUserSession userSession)
		{
			_puestosElectivosServices = puestosElectivosServices;
			_mapper = mapper;
			_userSession = userSession;
		}

		public async Task<IActionResult> Index()
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}
			var dtoList = await _puestosElectivosServices.GetAllAsync();
			var viewModelList = _mapper.Map<List<PuestoElectivoViewModel>>(dtoList);
			return View(viewModelList);
		}

		public IActionResult Create(int Id)
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}
			return View();
		}

		public async Task<IActionResult> Update(int Id)
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}

			var getPuestoElectivoId = await _puestosElectivosServices.GetByIdAsync(Id);
			var MappedEntity = _mapper.Map<PuestoElectivoViewModel>(getPuestoElectivoId);
			return View(MappedEntity);
		}

		[HttpPost]
		public async Task<IActionResult> Create(PuestoElectivoViewModel puestoElectivoViewModel)
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}

			if (!ModelState.IsValid) return View(puestoElectivoViewModel);

			var mappedEntity = _mapper.Map<PuestoElectivoDTO>(puestoElectivoViewModel);
			bool added = await _puestosElectivosServices.AddAsync(mappedEntity);

			if (!added)
			{
				ModelState.AddModelError(nameof(puestoElectivoViewModel.Nombre), "El nombre ya existe.");
				return View(puestoElectivoViewModel);
			}

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> ChangeStatus(int id)
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}
			var dto = await _puestosElectivosServices.GetByIdAsync(id);
			if (dto == null) return NotFound();

			var vm = _mapper.Map<PuestoElectivoViewModel>(dto);
			return View("ConfirmarEstado", vm);
		}

		// Acción para activar o desactivar
		[HttpPost]
		public async Task<IActionResult> ToggleEstado(int id)
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}
			var puesto = await _puestosElectivosServices.GetByIdAsync(id);
			if (puesto == null) return NotFound();

			bool nuevoEstado = !puesto.Estado;
			await _puestosElectivosServices.UpdateEstadoAsync(id, nuevoEstado);

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, PuestoElectivoViewModel vm)
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}

			if (!ModelState.IsValid)
			{
				return View(vm);
			}

			try
			{
				var dto = _mapper.Map<PuestoElectivoDTO>(vm);

				await _puestosElectivosServices.UpdateAsync(id, dto);

				return RedirectToAction("Index");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return View(vm);
			}
		}

	}
}
