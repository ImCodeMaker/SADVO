using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Application.Mappings;
using SADVO.Core.Application.Services;

namespace SADVO.Core.Application
{
	public static class ServiceLayerRegistration
	{
		public static void AddServicesLayerIOC(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(typeof(MappingProfiles));

			services.AddTransient(typeof(IGenericServices<,,,>), typeof(GenericService<,,,>));
			services.AddTransient<IUserServices, UsersServices>();
			services.AddTransient<IPuestosElectivosServices, PuestosElectivosServices>();
			services.AddTransient<ICiudadanosServices, CiudadanosServices>();
			services.AddTransient<IPartidoPoliticoServices, PartidosPoliticosServices>();
			services.AddTransient<IAsignacionDirigentesServices, AsignacionDirigentesServices>();
			services.AddTransient<ICandidatoServices, CandidatosServices>();
		}
	}
}
