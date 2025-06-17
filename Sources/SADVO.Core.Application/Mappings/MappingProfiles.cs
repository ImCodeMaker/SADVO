using AutoMapper;
using SADVO.Core.Application.Dtos.AsignacionDirigentes;
using SADVO.Core.Application.Dtos.Candidatos;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Application.ViewModels.AsignacionDirigentes;
using SADVO.Core.Application.ViewModels.Candidatos;
using SADVO.Core.Application.ViewModels.Ciudadanos;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;
using SADVO.Core.Application.ViewModels.PuestosElectivos;
using SADVO.Core.Application.ViewModels.Usuarios;
using SADVO.Core.Domain.Entities;
using System;

namespace SADVO.Core.Application.Mappings
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			#region Usuarios
			// Creation mappings
			CreateMap<CrearUsuarioDTO, Usuarios>().ReverseMap();

			// Update mappings
			CreateMap<UpdateUsuarioDTO, Usuarios>();


			// ViewModel mappings
			CreateMap<UsuarioDTO, UpdateUsuarioViewModel>()
				.ForMember(dest => dest.Contraseña, opt => opt.Ignore())
				.ForMember(dest => dest.ConfirmarContraseña, opt => opt.Ignore())
				.ReverseMap();  // Added ReverseMap for completeness

			CreateMap<Usuarios, UsuarioDTO>().ReverseMap();
			CreateMap<UsuarioViewModel, UsuarioDTO>().ReverseMap();

			CreateMap<UpdateUsuarioViewModel, UpdateUsuarioDTO>()
				.ForMember(dest => dest.Contraseña, opt => opt.MapFrom(src =>
					!string.IsNullOrEmpty(src.Contraseña) ? src.Contraseña : null))
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			// And make sure you have the reverse map too
			CreateMap<UpdateUsuarioDTO, UpdateUsuarioViewModel>()
				.ForMember(dest => dest.Contraseña, opt => opt.Ignore())
				.ForMember(dest => dest.ConfirmarContraseña, opt => opt.Ignore());

			CreateMap<CrearUsuarioViewModel, CrearUsuarioDTO>().ReverseMap();
			#endregion

			#region Login
			CreateMap<LoginDto, LoginViewModel>().ReverseMap();
			#endregion

			#region Puestos Electivos
			CreateMap<CrearPuestoElectivoDTO, PuestosElectivos>()
				.ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(_ => true));

			CreateMap<UpdatePuestoElectivoDTO, PuestosElectivos>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<PuestosElectivos, PuestoElectivoDTO>().ReverseMap();
			CreateMap<PuestoElectivoDTO, PuestoElectivoViewModel>().ReverseMap();
			CreateMap<CrearPuestoElectivoViewModel, CrearPuestoElectivoDTO>();
			CreateMap<UpdatePuestoElectivoViewModel, UpdatePuestoElectivoDTO>();
			CreateMap<PuestoElectivoDTO, UpdatePuestoElectivoViewModel>().ReverseMap();
			#endregion

			#region Ciudadanos
			CreateMap<CrearCiudadanos, Ciudadanos>()
				.ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(_ => true));

			CreateMap<UpdateCiudadanosDTO, Ciudadanos>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<Ciudadanos, CiudadanosDTO>().ReverseMap();
			CreateMap<CiudadanosDTO, CiudadanosViewModel>().ReverseMap();
			CreateMap<CrearCiudadanosViewModel, CrearCiudadanos>();
			CreateMap<UpdateCiudadanosViewModel, UpdateCiudadanosDTO>();
			CreateMap<CiudadanosDTO, UpdateCiudadanosViewModel>().ReverseMap();
			#endregion

			#region Partidos Politicos
			CreateMap<CrearPartidosPoliticosDTO, PartidosPoliticos>();

			CreateMap<UpdatePartidoPoliticoDTO, PartidosPoliticos>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<PartidosPoliticos, PartidosPoliticosDTO>().ReverseMap();
			CreateMap<PartidosPoliticosDTO, PartidosPoliticosViewModel>().ReverseMap();
			CreateMap<CrearPartidosPoliticosViewModel, CrearPartidosPoliticosDTO>();
			CreateMap<UpdatePartidosPoliticosViewModel, UpdatePartidoPoliticoDTO>();
			CreateMap<PartidosPoliticosDTO, UpdatePartidosPoliticosViewModel>();
			#endregion

			#region
			CreateMap<Candidatos, CandidatosDTO>().ReverseMap();
			CreateMap<CandidatosDTO, CandidatosViewModel>().ReverseMap();
			CreateMap<CrearCandidatosViewModel, CrearCandidatosDTO>();
			CreateMap<UpdateCandidatoViewModel, UpdateCiudadanosDTO>();
			CreateMap<CandidatosDTO, UpdateCandidatoDTO>();
			CreateMap<CrearCandidatosDTO, Candidatos>();
			CreateMap<CandidatosDTO, UpdateCandidatoViewModel>().ReverseMap();
			CreateMap<UpdateCandidatoDTO, Candidatos>().ReverseMap();
			CreateMap<UpdateCandidatoViewModel, UpdateCandidatoDTO>().ReverseMap();
			CreateMap<UpdateCandidatoDTO, Candidatos>()
				.ForMember(dest => dest.Foto, opt => opt.Ignore());

			#endregion

			#region AsignacionDirigentes
			CreateMap<AsignacionDirigentes, AsignacionDirigentesDTO>().ReverseMap();
			CreateMap<AsignacionDirigentesDTO, AsignacionDirigentesViewModel>().ReverseMap();
			CreateMap<CreateAsignacionDirigentesViewModel, CreateAsignacionDirigentesDTO>();

			CreateMap<CreateAsignacionDirigentesDTO, AsignacionDirigentes>()
				.ForMember(dest => dest.FechaAsignacion, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

			CreateMap<CreateAsignacionDirigentesViewModel, AsignacionDirigentes>()
				.ForMember(dest => dest.UsuarioName, opt => opt.MapFrom(src => src.UsuarioName))
				.ForMember(dest => dest.FechaAsignacion, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));
			#endregion


		}
	}
}