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
		public PartidosPoliticosController(IPartidoPoliticoServices partidoPoliticoServices, IMapper mapper, IUserSession userSession)
		{
			_partidoPoliticoServices = partidoPoliticoServices;
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
			var dtoList = await _partidoPoliticoServices.GetAllAsync();
			var viewModelList = _mapper.Map<List<PartidosPoliticosViewModel>>(dtoList);
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

			var getPartidoPoliticoId = await _partidoPoliticoServices.GetByIdAsync(Id);
			var MappedEntity = _mapper.Map<PartidosPoliticosViewModel>(getPartidoPoliticoId);
			return View(MappedEntity);
		}

		[HttpPost]
		public async Task<IActionResult> Create(PartidosPoliticosViewModel partidosPoliticosViewModel)
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
				return View(partidosPoliticosViewModel);

			var mappedDto = _mapper.Map<PartidosPoliticosDTO>(partidosPoliticosViewModel);

			bool added = await _partidoPoliticoServices.AddPartido(mappedDto, partidosPoliticosViewModel.LogoFile);

			if (!added)
			{
				ModelState.AddModelError("", "El documento o email ya ha sido registrado.");
				return View(partidosPoliticosViewModel);
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
			var dto = await _partidoPoliticoServices.GetByIdAsync(id);
			if (dto == null) return NotFound();

			var vm = _mapper.Map<PartidosPoliticosViewModel>(dto);
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
			var puesto = await _partidoPoliticoServices.GetByIdAsync(id);
			if (puesto == null) return NotFound();

			bool nuevoEstado = !puesto.Estado;
			await _partidoPoliticoServices.UpdateEstadoAsync(id, nuevoEstado);

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, PartidosPoliticosViewModel vm)
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
				var dto = _mapper.Map<PartidosPoliticosDTO>(vm);

				await _partidoPoliticoServices.UpdateAsync(id, dto);

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
