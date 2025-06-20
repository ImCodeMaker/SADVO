using AutoMapper;
using SADVO.Core.Application.Dtos.Alianzas;
using SADVO.Core.Application.Dtos.AsignacionCandidatos;
using SADVO.Core.Application.Dtos.AsignacionDirigentes;
using SADVO.Core.Application.Dtos.Candidatos;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Dtos.Elecciones;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Dtos.ResumenElectoral;
using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Application.Dtos.Votacion;
using SADVO.Core.Application.ViewModels.Alianzas;
using SADVO.Core.Application.ViewModels.AsignacionCandidatos;
using SADVO.Core.Application.ViewModels.AsignacionDirigentes;
using SADVO.Core.Application.ViewModels.Candidatos;
using SADVO.Core.Application.ViewModels.Ciudadanos;
using SADVO.Core.Application.ViewModels.Elecciones;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;
using SADVO.Core.Application.ViewModels.PuestosElectivos;
using SADVO.Core.Application.ViewModels.ResumenElectoral;
using SADVO.Core.Application.ViewModels.Usuarios;
using SADVO.Core.Application.ViewModels.Votacion;
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
			// Crear: ViewModel → DTO → Entidad
			CreateMap<CrearCiudadanosViewModel, CrearCiudadanos>();
			CreateMap<CrearCiudadanos, Ciudadanos>()
				.ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(_ => true));

			// Editar: ViewModel → DTO → Entidad
			CreateMap<UpdateCiudadanosViewModel, UpdateCiudadanosDTO>();
			CreateMap<UpdateCiudadanosDTO, Ciudadanos>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			// Entidad ↔ DTO
			CreateMap<Ciudadanos, CiudadanosDTO>().ReverseMap();

			// DTO ↔ ViewModel (para Index, Update GET, etc.)
			CreateMap<CiudadanosDTO, CiudadanosViewModel>().ReverseMap();
			CreateMap<CiudadanosDTO, UpdateCiudadanosViewModel>().ReverseMap();
			#endregion

			#region Partidos Politicos
			// Crear Partido: ViewModel → DTO → Entidad
			CreateMap<CrearPartidosPoliticosViewModel, CrearPartidosPoliticosDTO>();
			CreateMap<CrearPartidosPoliticosDTO, PartidosPoliticos>();

			// Editar Partido: ViewModel → DTO → Entidad (omitimos valores nulos)
			CreateMap<UpdatePartidosPoliticosViewModel, UpdatePartidoPoliticoDTO>();
			CreateMap<UpdatePartidoPoliticoDTO, PartidosPoliticos>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			// Mapeo general Entidad ↔ DTO para acceso de servicio
			CreateMap<PartidosPoliticos, PartidosPoliticosDTO>().ReverseMap();

			// DTO ↔ ViewModel para vistas como Index, ConfirmarEstado, etc.
			CreateMap<PartidosPoliticosDTO, PartidosPoliticosViewModel>().ReverseMap();

			// Para vista Update: DTO → ViewModel
			CreateMap<PartidosPoliticosDTO, UpdatePartidosPoliticosViewModel>();

			#endregion

			#region Candidatos
			// ViewModel → DTO
			CreateMap<CrearCandidatosViewModel, CrearCandidatosDTO>();
			CreateMap<UpdateCandidatoViewModel, UpdateCandidatoDTO>();

			// DTO → ViewModel 
			CreateMap<CandidatosDTO, CandidatosViewModel>();
			CreateMap<CandidatosDTO, UpdateCandidatoViewModel>();

			// DTO → Entidad
			CreateMap<CrearCandidatosDTO, Candidatos>();
			CreateMap<UpdateCandidatoDTO, Candidatos>()
				.ForMember(dest => dest.Foto, opt => opt.Ignore()) 
				.ForMember(dest => dest.PartidoName, opt => opt.Ignore())
				.ForMember(dest => dest.PartidoPoliticoId, opt => opt.Ignore());

			// Entidad → DTO (para traer desde la base)
			CreateMap<Candidatos, CandidatosDTO>();


			#endregion

			#region AsignacionDirigentes
			// Entidad ↔ DTO
			CreateMap<AsignacionDirigentes, AsignacionDirigentesDTO>().ReverseMap();

			// DTO ↔ ViewModel (para Index, ConfirmarEstado, etc.)
			CreateMap<AsignacionDirigentesDTO, AsignacionDirigentesViewModel>().ReverseMap();

			// Crear flujo: ViewModel → DTO → Entidad
			CreateMap<CreateAsignacionDirigentesViewModel, CreateAsignacionDirigentesDTO>();
			CreateMap<CreateAsignacionDirigentesDTO, AsignacionDirigentes>()
				.ForMember(dest => dest.FechaAsignacion, opt => opt.MapFrom(_ => DateTime.Now))
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

			#endregion

			#region AlianzasPoliticas
			CreateMap<AlianzasPoliticas, AlianzasDTO>()
				.ForMember(dest => dest.NombrePartidoSolicitante, opt => opt.MapFrom(src => src.PartidoSolicitante.Nombre))
				.ForMember(dest => dest.SiglasPartidoSolicitante, opt => opt.MapFrom(src => src.PartidoSolicitante.Siglas))
				.ForMember(dest => dest.NombrePartidoDestino, opt => opt.MapFrom(src => src.PartidoDestino.Nombre))
				.ForMember(dest => dest.SiglasPartidoDestino, opt => opt.MapFrom(src => src.PartidoDestino.Siglas));

			// DTO to Entity
			CreateMap<CrearAlianzaDTO, AlianzasPoliticas>();
			CreateMap<UpdateAlianzaDTO, AlianzasPoliticas>();
			CreateMap<AlianzasDTO, UpdateAlianzaDTO>();


			// ViewModel to DTO
			CreateMap<CrearAlianzaViewModel, CrearAlianzaDTO>();

			// DTO to ViewModel
			CreateMap<AlianzasDTO, CrearAlianzaViewModel>();

			#endregion

			#region Asignaciones Candidatos
			CreateMap<AsignacionCandidatos, AsignacionCandidatoDTO>()
			.ForMember(dest => dest.NombreCandidato, opt => opt.MapFrom(src => src.Candidato.Nombre))
			.ForMember(dest => dest.ApellidoCandidato, opt => opt.MapFrom(src => src.Candidato.Apellido))
			.ForMember(dest => dest.NombrePuestoElectivo, opt => opt.MapFrom(src => src.puestosElectivos.Nombre))
			.ForMember(dest => dest.NombrePartidoPolitico, opt => opt.MapFrom(src => src.PartidosPoliticos.Nombre))
			.ForMember(dest => dest.SiglasPartidoPolitico, opt => opt.MapFrom(src => src.PartidosPoliticos.Siglas))
			.ForMember(dest => dest.NombrePartidoQueRespalda, opt => opt.MapFrom(src => src.PartidoQueRespalda != null ? src.PartidoQueRespalda.Nombre : null))
			.ForMember(dest => dest.SiglasPartidoQueRespalda, opt => opt.MapFrom(src => src.PartidoQueRespalda != null ? src.PartidoQueRespalda.Siglas : null));

			CreateMap<CrearAsignacionCandidatoDTO, AsignacionCandidatos>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.FechaAsignacion, opt => opt.MapFrom(src => DateTime.Now))
				.ForMember(dest => dest.Candidato, opt => opt.Ignore())
				.ForMember(dest => dest.puestosElectivos, opt => opt.Ignore())
				.ForMember(dest => dest.PartidosPoliticos, opt => opt.Ignore());

			CreateMap<UpdateAsignacionCandidatoDTO, AsignacionCandidatos>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.FechaAsignacion, opt => opt.Ignore())
				.ForMember(dest => dest.Candidato, opt => opt.Ignore())
				.ForMember(dest => dest.puestosElectivos, opt => opt.Ignore())
				.ForMember(dest => dest.PartidosPoliticos, opt => opt.Ignore());

			CreateMap<AsignacionCandidatoDTO, AsignacionCandidatoViewModel>();

			CreateMap<CrearAsignacionCandidatoViewModel, CrearAsignacionCandidatoDTO>();

			CreateMap<AsignacionCandidatoDTO, ConfirmarEliminarAsignacionViewModel>()
				.ForMember(dest => dest.NombreCandidato, opt => opt.MapFrom(src => src.NombreCandidato))
				.ForMember(dest => dest.ApellidoCandidato, opt => opt.MapFrom(src => src.ApellidoCandidato))
				.ForMember(dest => dest.NombrePuestoElectivo, opt => opt.MapFrom(src => src.NombrePuestoElectivo));
			#endregion

			#region Elecciones
			CreateMap<Elecciones, EleccionDTO>()
				.ForMember(dest => dest.EsActiva, opt => opt.MapFrom(src => src.Estado ));

			CreateMap<EleccionDTO, EleccionViewModel>();

			CreateMap<CrearEleccionDTO, Elecciones>()
				.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now));

			CreateMap<CreateEleccionViewModel, CrearEleccionDTO>();

			CreateMap<UpdateEleccionDTO, Elecciones>();

			CreateMap<ResultadoEleccion, ResultadoEleccionDTO>();
			#endregion

			#region Votaciones

			CreateMap<Elecciones, EleccionVotacionDTO>()
			   .ForMember(dest => dest.EleccionId, opt => opt.MapFrom(src => src.Id))
			   .ForMember(dest => dest.NombreEleccion, opt => opt.MapFrom(src => src.Nombre))
			   .ForMember(dest => dest.PuestosElectivos, opt => opt.Ignore())
			   .ForMember(dest => dest.YaVotoCompleto, opt => opt.Ignore());

			CreateMap<AsignacionCandidatos, CandidatoVotacionDTO>()
				.ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.CandidatoId))
				.ForMember(dest => dest.NombreCandidato, opt => opt.MapFrom(src => src.Candidato.Nombre))
				.ForMember(dest => dest.ApellidoCandidato, opt => opt.MapFrom(src => src.Candidato.Apellido))
				.ForMember(dest => dest.FotoCandidato, opt => opt.MapFrom(src => src.Candidato.Foto))
				.ForMember(dest => dest.PuestoElectivoId, opt => opt.MapFrom(src => src.PuestoElectivoId))
				.ForMember(dest => dest.NombrePuestoElectivo, opt => opt.MapFrom(src => src.puestosElectivos.Nombre))
				.ForMember(dest => dest.PartidoPrincipalId, opt => opt.MapFrom(src => src.PartidoPoliticoId))
				.ForMember(dest => dest.NombrePartidoPrincipal, opt => opt.MapFrom(src => src.PartidosPoliticos.Nombre))
				.ForMember(dest => dest.SiglasPartidoPrincipal, opt => opt.MapFrom(src => src.PartidosPoliticos.Siglas))
				.ForMember(dest => dest.PartidosRespaldo, opt => opt.Ignore());

			CreateMap<AsignacionCandidatos, PartidoRespaldoDTO>()
				.ForMember(dest => dest.PartidoId, opt => opt.MapFrom(src => src.PartidoRespaldaId!.Value))
				.ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.PartidoQueRespalda!.Nombre))
				.ForMember(dest => dest.Siglas, opt => opt.MapFrom(src => src.PartidoQueRespalda!.Siglas));

			CreateMap<PuestosElectivos, PuestoElectivoVotacionDTO>()
				.ForMember(dest => dest.PuestoElectivoId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.Nombre))
				.ForMember(dest => dest.Candidatos, opt => opt.Ignore())
				.ForMember(dest => dest.YaVotado, opt => opt.Ignore());

			// Mapeos de DTO a ViewModel
			CreateMap<EleccionVotacionDTO, EleccionVotacionViewModel>();

			CreateMap<CandidatoVotacionDTO, CandidatoVotacionViewModel>();

			CreateMap<PartidoRespaldoDTO, PartidoRespaldoViewModel>();

			CreateMap<PuestoElectivoVotacionDTO, PuestoElectivoVotacionViewModel>();

			CreateMap<ResultadoVotoDTO, ResultadoVotoViewModel>();

			// Mapeos de ViewModel a DTO
			CreateMap<EleccionVotacionViewModel, EleccionVotacionDTO>();

			CreateMap<CandidatoVotacionViewModel, CandidatoVotacionDTO>();

			CreateMap<PartidoRespaldoViewModel, PartidoRespaldoDTO>();

			CreateMap<PuestoElectivoVotacionViewModel, PuestoElectivoVotacionDTO>();

			CreateMap<RegistrarVotoViewModel, RegistrarVotoDTO>();

			CreateMap<ResultadoVotoViewModel, ResultadoVotoDTO>();

			// Mapeos de DTO a Entity
			CreateMap<RegistrarVotoDTO, Votos>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0))
				.ForMember(dest => dest.FechaVoto, opt => opt.MapFrom(src => DateTime.Now))
				.ForMember(dest => dest.Eleccion, opt => opt.Ignore())
				.ForMember(dest => dest.Ciudadano, opt => opt.Ignore())
				.ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore())
				.ForMember(dest => dest.Candidato, opt => opt.Ignore())
				.ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore());

			CreateMap<RegistrarVotoDTO, HistorialVotaciones>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0))
				.ForMember(dest => dest.HaVotado, opt => opt.MapFrom(src => true))
				.ForMember(dest => dest.FechaVotacion, opt => opt.MapFrom(src => DateTime.Now))
				.ForMember(dest => dest.EmailEnviado, opt => opt.MapFrom(src => false))
				.ForMember(dest => dest.Eleccion, opt => opt.Ignore())
				.ForMember(dest => dest.Ciudadano, opt => opt.Ignore());

			// Mapeos inversos (Entity a DTO)
			CreateMap<Votos, RegistrarVotoDTO>();

			CreateMap<HistorialVotaciones, RegistrarVotoDTO>()
				.ForMember(dest => dest.PuestoElectivoId, opt => opt.Ignore())
				.ForMember(dest => dest.CandidatoId, opt => opt.Ignore())
				.ForMember(dest => dest.PartidoPoliticoId, opt => opt.Ignore());
			#endregion

			#region ResumenElectoral
			CreateMap<ResumenElectoralDTO, EleccionResumenViewModel>();
			#endregion
		}
	}
}