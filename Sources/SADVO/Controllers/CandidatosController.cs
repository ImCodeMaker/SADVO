using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Candidatos;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.ViewModels.Candidatos;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;

namespace SADVO.Controllers
{
	public class CandidatosController : Controller
	{
		private readonly ICandidatoServices _candidatosServices;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;
		private readonly IAsignacionDirigentesServices _asignacionDirigentesServices;
		private readonly IPartidoPoliticoServices _partidoPoliticoServices;
		private readonly IEleccionesServices _eleccionesServices;

		public CandidatosController(ICandidatoServices candidatosServices, IMapper mapper, IUserSession userSession, IAsignacionDirigentesServices asignacionDirigentesServices, IPartidoPoliticoServices partidoPoliticoServices, IEleccionesServices eleccionesServices)
		{
			_candidatosServices = candidatosServices;
			_mapper = mapper;
			_userSession = userSession;
			_asignacionDirigentesServices = asignacionDirigentesServices;
			_partidoPoliticoServices = partidoPoliticoServices;
			_partidoPoliticoServices = partidoPoliticoServices;
			_eleccionesServices = eleccionesServices;
		}

		// Método helper para verificar autorización (como en tu otro controller)
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

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var dtoList = await _candidatosServices.GetAllAsync();
			var viewModelList = _mapper.Map<List<CandidatosViewModel>>(dtoList);

			foreach (var vm in viewModelList)
			{
				vm.TieneAsignacion = await _candidatosServices.TienePuestoElectivo(vm.Id);
			}

			return View(viewModelList);
		}


		// GET: Create
		public IActionResult Create()
		{
			var authResult = CheckAuthorization();
			return authResult ?? View();
		}

		// POST: Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CrearCandidatosViewModel crearCandidatosViewModel)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
				return View(crearCandidatosViewModel);

			try
			{
				var user = _userSession.GetUserSession();
				var userId = user.Id;
				var dirigiente = await _asignacionDirigentesServices.GetDirigente(userId);
				var getPartido = await _partidoPoliticoServices.GetByIdAsync(dirigiente.PartidoPoliticoId);

				var mappedDto = _mapper.Map<CrearCandidatosDTO>(crearCandidatosViewModel);
				bool added = await _candidatosServices.AddCandidato(mappedDto, crearCandidatosViewModel.LogoFile, dirigiente.PartidoPoliticoId, getPartido!.Nombre);

				if (!added)
				{
					ModelState.AddModelError("", "El documento o email ya ha sido registrado.");
					return View(crearCandidatosViewModel);
				}

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Error al crear el partido político: " + ex.Message);
				return View(crearCandidatosViewModel);
			}
		}

		public async Task<IActionResult> Update(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (id <= 0)
			{
				TempData["Error"] = "ID inválido.";
				return RedirectToAction("Index");
			}

			try
			{
				var getCandidatoId = await _candidatosServices.GetByIdAsync(id);

				if (getCandidatoId == null)
				{
					return NotFound();
				}

				var mappedEntity = _mapper.Map<UpdateCandidatoViewModel>(getCandidatoId);

				if (mappedEntity == null)
				{
					TempData["Error"] = "Error al mapear este candidato.";
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, UpdateCandidatoViewModel vm)
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
				var dto = _mapper.Map<UpdateCandidatoDTO>(vm);

				bool updated = await _candidatosServices.UpdateAsync(id, dto, vm.LogoFile);

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
				return View("Update", vm); 
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Error al actualizar el partido político: " + ex.Message);
				return View("Update", vm); 
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
				var dto = await _candidatosServices.GetByIdAsync(id);

				if (dto == null)
					return NotFound();

				var vm = _mapper.Map<CandidatosViewModel>(dto);
				return View("ConfirmarEstado", vm);
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al cargar el partido político: " + ex.Message;
				return RedirectToAction("Index");
			}
		}

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
				var partido = await _candidatosServices.GetByIdAsync(id);

				if (partido == null)
					return NotFound();

				bool nuevoEstado = !partido.Estado;
				await _candidatosServices.UpdateEstadoAsync(id, nuevoEstado);

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