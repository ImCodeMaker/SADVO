using AutoMapper;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Application.ViewModels.Ciudadanos;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;
using SADVO.Core.Application.ViewModels.PuestosElectivos;
using SADVO.Core.Application.ViewModels.Usuarios;
using SADVO.Core.Domain.Entities;
namespace SADVO.Core.Application.Mappings
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			#region Usuarios
			CreateMap<Usuarios, UsuarioDTO>().ReverseMap()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<Usuarios, CrearUsuarioDTO>().ReverseMap();
			#endregion

			#region Login
			CreateMap<LoginDto, LoginViewModel>().ReverseMap();
			#endregion

			#region ViewModels
			CreateMap<UserViewModel, UsuarioDTO>().ReverseMap();
			#endregion

			#region Puestos Electivos
			CreateMap<PuestosElectivos, PuestoElectivoDTO>().ReverseMap();
			CreateMap<PuestoElectivoDTO, PuestoElectivoViewModel>().ReverseMap();
			#endregion

			#region Ciudadanos
			CreateMap<Ciudadanos,CiudadanosDTO>().ReverseMap();
			CreateMap<CiudadanosDTO, CiudadanosViewModel>().ReverseMap();
			#endregion

			#region PartidosPoliticos
			CreateMap<PartidosPoliticos, PartidosPoliticosDTO>().ReverseMap();
			CreateMap<PartidosPoliticosDTO, PartidosPoliticosViewModel>().ReverseMap();		
			#endregion
		}
	}

}
