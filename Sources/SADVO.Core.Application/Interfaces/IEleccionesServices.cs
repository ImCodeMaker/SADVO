using SADVO.Core.Application.Dtos.Elecciones;
using SADVO.Core.Application.Dtos.ResumenElectoral;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IEleccionesServices : IGenericServices<CrearEleccionDTO, UpdateEleccionDTO, EleccionDTO, Elecciones>
	{
		Task<EleccionDTO?> GetEleccionActivaAsync();
		Task<List<EleccionDTO>> GetAllEleccionesOrderedAsync();
		Task<bool> FinalizarEleccionAsync(int eleccionId);
		Task<(bool isValid, List<string> errors)> ValidarCreacionEleccionAsync();
		Task<List<ResultadoEleccionDTO>> GetResultadosEleccionAsync(int eleccionId);
		Task<(bool success, List<string> errors)> CreateEleccionWithValidationAsync(CrearEleccionDTO dto);
		Task<AniosDisponiblesDTO> GetAniosDisponiblesAsync();
		Task<List<ResumenElectoralDTO>> GetResumenElectoralPorAnioAsync(int anio);
		Task<(bool isValid, List<string> errors)> ValidarCreacionEleccionConAnioAsync(CrearEleccionDTO dto);
		Task<bool> HayEleccionActivaAsync();

	}
}
