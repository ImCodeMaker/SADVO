using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Ciudadanos;

namespace SADVO.Controllers
{
	public class CiudadanosController : Controller
	{
		private readonly ICiudadanosServices _ciudadanosServices;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;
		public CiudadanosController(ICiudadanosServices ciudadanosServices, IMapper mapper, IUserSession userSession)
		{
			_ciudadanosServices = ciudadanosServices;
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
			var dtoList = await _ciudadanosServices.GetAllAsync();
			var viewModelList = _mapper.Map<List<CiudadanosViewModel>>(dtoList);
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

			var getPuestoElectivoId = await _ciudadanosServices.GetByIdAsync(Id);
			var MappedEntity = _mapper.Map<CiudadanosViewModel>(getPuestoElectivoId);
			return View(MappedEntity);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CiudadanosViewModel ciudadanosViewModel)
		{
			if (!_userSession.hasUser())
			{
				return RedirectToRoute(new { controller = "Auth", action = "Index" });
			}

			if (!_userSession.checkRole())
			{
				return RedirectToRoute(new { controller = "Auth", action = "AcessDenied" });
			}

			if (!ModelState.IsValid) return View(ciudadanosViewModel);

			var mappedEntity = _mapper.Map<CiudadanosDTO>(ciudadanosViewModel);
			bool added = await _ciudadanosServices.AddAsync(mappedEntity);

			if (!added)
			{
				ModelState.AddModelError("", "El documento o email ya ha sido registrado.");
				return View(ciudadanosViewModel);
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
			var dto = await _ciudadanosServices.GetByIdAsync(id);
			if (dto == null) return NotFound();

			var vm = _mapper.Map<CiudadanosViewModel>(dto);
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
			var puesto = await _ciudadanosServices.GetByIdAsync(id);
			if (puesto == null) return NotFound();

			bool nuevoEstado = !puesto.Estado;
			await _ciudadanosServices.UpdateEstadoAsync(id, nuevoEstado);

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, CiudadanosViewModel vm)
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
				var dto = _mapper.Map<CiudadanosDTO>(vm);

				await _ciudadanosServices.UpdateAsync(id, dto);

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
