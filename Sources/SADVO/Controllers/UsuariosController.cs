using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.Services;
using SADVO.Core.Application.ViewModels.Ciudadanos;
using SADVO.Core.Application.ViewModels.Usuarios;

namespace SADVO.Controllers
{
	public class UsuariosController : Controller
	{
		private readonly IUserServices _userServices;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;

		public UsuariosController(IUserServices userServices, IMapper mapper, IUserSession userSession)
		{
			_userServices = userServices;
			_mapper = mapper;
			_userSession = userSession;
		}

		private IActionResult RedirectIfNotAuthenticated()
		{
			if (!_userSession.hasUser())
				return RedirectToAction("Index", "Auth");

			if (!_userSession.checkRole())
				return RedirectToAction("AcessDenied", "Auth");

			return null;
		}

		public async Task<IActionResult> Index()
		{
			var authCheck = RedirectIfNotAuthenticated();
			if (authCheck != null) return authCheck;

			var users = await _userServices.GetAllAsync();
			return View(_mapper.Map<List<UsuarioViewModel>>(users));
		}

		public IActionResult Create()
		{
			var authCheck = RedirectIfNotAuthenticated();
			return authCheck ?? View();
		}

		public async Task<IActionResult> Update(int id)
		{
			var authCheck = RedirectIfNotAuthenticated();
			if (authCheck != null) return authCheck;

			var user = await _userServices.GetByIdAsync(id);
			if (user == null) return NotFound();

			return View(_mapper.Map<UpdateUsuarioViewModel>(user));
		}

		[HttpPost]
		public async Task<IActionResult> Create(CrearUsuarioViewModel model)
		{
			var authCheck = RedirectIfNotAuthenticated();
			if (authCheck != null) return authCheck;

			if (!ModelState.IsValid)
				return View(model);

			try
			{
				var result = await _userServices.AddAsync(_mapper.Map<CrearUsuarioDTO>(model));
				return result ? RedirectToAction("Index") : throw new InvalidOperationException("User already exists");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, UpdateUsuarioViewModel model)
		{
			var authResult = RedirectIfNotAuthenticated();
			if (authResult != null) return authResult;

			if (!ModelState.IsValid)
				return View("Update", model);

			try
			{
				var updateDto = _mapper.Map<UpdateUsuarioDTO>(model);
				await _userServices.UpdateAsync(id, updateDto);

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
			var authCheck = RedirectIfNotAuthenticated();
			if (authCheck != null) return authCheck;

			var user = await _userServices.GetByIdAsync(id);
			return user == null ? NotFound() : View("ConfirmarEstado", _mapper.Map<UsuarioViewModel>(user));
		}

		[HttpPost]
		public async Task<IActionResult> ToggleEstado(int id)
		{
			var authCheck = RedirectIfNotAuthenticated();
			if (authCheck != null) return authCheck;

			var user = await _userServices.GetByIdAsync(id);
			if (user == null) return NotFound();

			await _userServices.UpdateEstadoAsync(id, !user.Estado);
			return RedirectToAction("Index");
		}
	}
}