using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;
using SADVO.Infrastructure.Persistence.Repositories;


namespace SADVO.Infrastructure.Persistence
{
	public static class ServicesRegistration
	{
		public static void AddPersistenceLayerIOC(this IServiceCollection services, IConfiguration configuration)
		{
			#region Contexts
			services.AddDbContext<SADVODbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
				ServiceLifetime.Scoped);
			#endregion

			#region Repositories IOC
			services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IPuestosElectivosRepository, PuestosElectivosRepository>();
			services.AddTransient<ICiudadanosRepository, CiudadanosRepository>();
			services.AddTransient<IPartidosPoliticosRepository, PartidosPoliticosRepository>();
			services.AddTransient<IAsignacionDirigentesRepository, AsignacionDirigentesRepository>();
			services.AddTransient<ICandidatosRepository, CandidatosRepository>();
			#endregion
		}

	}
}
