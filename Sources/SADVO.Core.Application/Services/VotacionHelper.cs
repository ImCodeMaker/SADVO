using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class VotacionHelper : IVotacionHelperServices
	{
		private readonly ICiudadanosServices _ciudadanosService;

		public VotacionHelper(ICiudadanosServices ciudadanosService)
		{
			_ciudadanosService = ciudadanosService;
		}

		public async Task<CiudadanosDTO?> GetCiudadanosById(int Id)
		{
			return await _ciudadanosService.GetByIdAsync(Id);
		}
	}
}
