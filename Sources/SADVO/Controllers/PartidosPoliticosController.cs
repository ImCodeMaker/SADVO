using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Ciudadanos;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;

namespace SADVO.Controllers
{
	public class PartidosPoliticosController : Controller
	{
		private readonly IPartidoPoliticoServices _partidoPoliticoServices;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;
		private readonly IEleccionesServices _eleccionesServices;

		public PartidosPoliticosController(IPartidoPoliticoServices partidoPoliticoServices, IMapper mapper, IUserSession userSession, IEleccionesServices eleccionesServices)
		{
			_partidoPoliticoServices = partidoPoliticoServices;
			_mapper = mapper;
			_userSession = userSession;
			_eleccionesServices = eleccionesServices;

		}

		// Método helper para verificar autorización (como en tu otro controller)
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

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var dtoList = await _partidoPoliticoServices.GetAllAsync();
			var viewModelList = _mapper.Map<List<PartidosPoliticosViewModel>>(dtoList);
			return View(viewModelList);
		}

		// GET: Create
		public IActionResult Create()
		{
			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var authResult = CheckAuthorization();
			return authResult ?? View();
		}

		// POST: Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CrearPartidosPoliticosViewModel partidosPoliticosViewModel)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
				return View(partidosPoliticosViewModel);

			try
			{
				var mappedDto = _mapper.Map<CrearPartidosPoliticosDTO>(partidosPoliticosViewModel);
				bool added = await _partidoPoliticoServices.AddPartido(mappedDto, partidosPoliticosViewModel.LogoFile);

				if (!added)
				{
					ModelState.AddModelError("", "El documento o email ya ha sido registrado.");
					return View(partidosPoliticosViewModel);
				}

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Error al crear el partido político: " + ex.Message);
				return View(partidosPoliticosViewModel);
			}
		}

		// GET: Update (cambiamos el nombre del método para que coincida con tu patrón)
		public async Task<IActionResult> Update(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			// Debug: Verificar que el ID llega correctamente
			if (id <= 0)
			{
				TempData["Error"] = "ID inválido.";
				return RedirectToAction("Index");
			}

			try
			{
				var getPartidoPoliticoId = await _partidoPoliticoServices.GetByIdAsync(id);

				if (getPartidoPoliticoId == null)
				{
					return NotFound();
				}

				// FIXED: Map to UpdatePartidosPoliticosViewModel instead of PartidosPoliticosViewModel
				var mappedEntity = _mapper.Map<UpdatePartidosPoliticosViewModel>(getPartidoPoliticoId);

				if (mappedEntity == null)
				{
					TempData["Error"] = "Error al mapear el modelo del partido político.";
					return RedirectToAction("Index");
				}

				return View(mappedEntity);
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al cargar el partido político: " + ex.Message;
				return RedirectToAction("Index");
			}
		}

		// POST: Edit (este método procesa la edición)
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, UpdatePartidosPoliticosViewModel vm)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
			{
				// IMPORTANTE: Devolver la vista "Update" con el modelo
				return View("Update", vm);
			}

			try
			{
				var dto = _mapper.Map<UpdatePartidoPoliticoDTO>(vm);

				// ✅ USAR EL NUEVO MÉTODO que maneja archivos
				bool updated = await _partidoPoliticoServices.UpdateAsync(id, dto, vm.LogoFile);

				if (updated)
				{
					TempData["Success"] = "Partido político actualizado correctamente.";
					return RedirectToAction("Index");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Error al actualizar el partido político.");
					return View("Update", vm);
				}
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return View("Update", vm); // Devolver la vista "Update"
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Error al actualizar el partido político: " + ex.Message);
				return View("Update", vm); // Devolver la vista "Update"
			}
		}

		public async Task<IActionResult> ChangeStatus(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			try
			{
				var dto = await _partidoPoliticoServices.GetByIdAsync(id);

				if (dto == null)
					return NotFound();

				var vm = _mapper.Map<PartidosPoliticosViewModel>(dto);
				return View("ConfirmarEstado", vm);
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al cargar el partido político: " + ex.Message;
				return RedirectToAction("Index");
			}
		}

		// POST: Toggle Estado
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ToggleEstado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			try
			{
				var partido = await _partidoPoliticoServices.GetByIdAsync(id);

				if (partido == null)
					return NotFound();

				bool nuevoEstado = !partido.Estado;
				await _partidoPoliticoServices.UpdateEstadoAsync(id, nuevoEstado);

				TempData["Success"] = "Estado del partido político actualizado correctamente.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al cambiar el estado: " + ex.Message;
				return RedirectToAction("Index");
			}
		}
	}
}