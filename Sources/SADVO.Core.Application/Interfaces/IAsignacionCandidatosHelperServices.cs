using SADVO.Core.Application.Dtos.AsignacionCandidatos;
using SADVO.Core.Application.Dtos.Candidatos;
using SADVO.Core.Application.Dtos.PuestoElectivo;

namespace SADVO.Core.Application.Interfaces
{
	public interface IAsignacionCandidatosHelperService
	{
		Task<bool> ValidarCandidatoParaAsignacionAsync(int candidatoId, int puestoElectivoId, int partidoId);
		Task<bool> CandidatoPerteneceAPartidoAliado(int candidatoId, int partidoId);
		Task<List<PuestoElectivoDTO>> GetPuestosDisponiblesAsync(int partidoId, int? candidatoId = null);
		Task<List<CandidatosDTO>> GetCandidatosDisponiblesAsync(int partidoId);
		Task<List<AsignacionCandidatoDTO>> GetAsignacionesConAliadosAsync(int partidoId);


	}
}