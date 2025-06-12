using AutoMapper;
using SADVO.Core.Application.Dtos;
using SADVO.Core.Application.ViewModels;
using SADVO.Core.Domain.Entities;
namespace SADVO.Core.Application.Mappings
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Usuarios, UsuarioDTO>().ReverseMap()
			.ForAllMembers(opts => opts.Condition(
				(src, dest, srcMember) => srcMember != null));

			CreateMap<Usuarios, CrearUsuarioDTO>().ReverseMap();
			CreateMap<LoginDto, LoginViewModel>().ReverseMap();
			CreateMap<UserViewModel, UsuarioDTO>().ReverseMap();
		}
	}
}
