using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;

namespace SADVO.Middlewares
{
	public class CiudadanoSession : ICiudadanoSession
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CiudadanoSession(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public bool hasUser()
		{
			CiudadanosDTO? ciudadanosViewModel = _httpContextAccessor.HttpContext?.Session.Get<CiudadanosDTO>("Ciudadano");

			if (ciudadanosViewModel == null)
			{
				return false;
			}
			return true;
		}

		public CiudadanosDTO GetCiudadanoSession()
		{
			CiudadanosDTO? ciudadanosViewModel = _httpContextAccessor.HttpContext?.Session.Get<CiudadanosDTO>("Ciudadano");

			if (ciudadanosViewModel == null)
			{
				return null!;
			}
			return ciudadanosViewModel;
		}

	}
}
