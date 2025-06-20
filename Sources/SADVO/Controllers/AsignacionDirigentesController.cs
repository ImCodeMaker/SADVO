using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SADVO.Core.Application.Dtos.AsignacionDirigentes;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.Services;
using SADVO.Core.Application.ViewModels.AsignacionDirigentes;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;
using SADVO.Core.Application.ViewModels.Usuarios;

namespace SADVO.Controllers
{
	public class AsignacionDirigentesController : Controller
	{
		private readonly IAsignacionDirigentesServices _asignacionDirigentesService;
		private readonly IUserServices _usuariosService;
		private readonly IPartidoPoliticoServices _partidosPoliticosService;
		private readonly IMapper _mapper;
		private readonly IUserSession _userSession;
		private readonly IEleccionesServices _eleccionesServices;

		public AsignacionDirigentesController(
			IAsignacionDirigentesServices asignacionDirigentesService,
		IUserServices usuariosService,
			IPartidoPoliticoServices partidosPoliticosService,
			IMapper mapper,
			IUserSession userSession,
			IEleccionesServices eleccionesService)
		{
			_asignacionDirigentesService = asignacionDirigentesService;
			_usuariosService = usuariosService;
			_partidosPoliticosService = partidosPoliticosService;
			_mapper = mapper;
			_userSession = userSession;
			_eleccionesServices = eleccionesService;
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

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var items = await _asignacionDirigentesService.GetAllAsync();
			var viewModel = _mapper.Map<List<AsignacionDirigentesViewModel>>(items);
			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var usuarios = await _usuariosService.GetActiveUsersAsync();
			var partidos = await _partidosPoliticosService.GetActivePartidosAsync();

			var model = new CreateAsignacionDirigentesViewModel
			{
				Estado = true,
				UsuariosActivos = _mapper.Map<List<UsuarioViewModel>>(usuarios),
				PartidosActivos = _mapper.Map<List<PartidosPoliticosViewModel>>(partidos)
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateAsignacionDirigentesViewModel model)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			if (!ModelState.IsValid)
			{
				var usuarios = await _usuariosService.GetActiveUsersAsync();
				var partidos = await _partidosPoliticosService.GetActivePartidosAsync();

				model.UsuariosActivos = _mapper.Map<List<UsuarioViewModel>>(usuarios);
				model.PartidosActivos = _mapper.Map<List<PartidosPoliticosViewModel>>(partidos);

				return View(model);
			}

			var usuario = await _usuariosService.GetByIdAsync(model.UsuarioId);
			if (usuario == null)
			{
				ModelState.AddModelError(string.Empty, "El usuario no existe.");
				return View(model);
			}

			model.UsuarioName = usuario.NombreUsuario;

			var dto = _mapper.Map<CreateAsignacionDirigentesDTO>(model);
			await _asignacionDirigentesService.AddAsync(dto);

			return RedirectToAction("Index");
		}


		[HttpPost]
		public async Task<IActionResult> ToggleEstado(int id)
		{
			var authResult = CheckAuthorization();
			if (authResult != null) return authResult;

			if (_eleccionesServices.GetEleccionActivaAsync != null)
			{
				return RedirectToAction("PeriodoElectoral", "Auth");
			}

			var entity = await _asignacionDirigentesService.GetByIdAsync(id);
			if (entity == null) return NotFound();

			await _asignacionDirigentesService.UpdateEstadoAsync(id, !entity.Estado);
			return RedirectToAction("Index");
		}
	}
}
