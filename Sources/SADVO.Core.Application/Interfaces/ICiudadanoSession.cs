using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.ViewModels.Ciudadanos;

namespace SADVO.Core.Application.Interfaces
{
	public interface ICiudadanoSession
	{
		bool hasUser();
		CiudadanosDTO GetCiudadanoSession();
	}
}
