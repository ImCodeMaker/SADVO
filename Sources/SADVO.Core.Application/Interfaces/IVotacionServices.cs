using SADVO.Core.Application.Dtos.Votacion;

namespace SADVO.Core.Application.Interfaces
{
	public interface IVotacionService
	{
		Task<EleccionVotacionDTO?> GetEleccionParaVotarAsync(int ciudadanoId);
		Task<ResultadoVotoDTO> RegistrarVotoAsync(RegistrarVotoDTO dto);
		Task<bool> PuedeVotarAsync(int ciudadanoId);
	}
}