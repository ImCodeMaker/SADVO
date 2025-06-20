
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SADVO.Core.Application.Interfaces;
using SADVO.Infrastructure.Shared.Services;

namespace SADVO.Infrastructure.Shared
{
	public static class ServiceLayerRegistration
	{
		public static void AddSharedLayerIOC(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<IEmailServices, EmailService>();
		}
	}
}
