using SADVO.Core.Application.Dtos.AsignacionCandidatos;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IAsignacionCandidatosServices : IGenericServices<CrearAsignacionCandidatoDTO, UpdateAsignacionCandidatoDTO, AsignacionCandidatoDTO, AsignacionCandidatos>
	{
		Task<List<AsignacionCandidatoDTO>> GetAsignacionesByPartidoAsync(int partidoId);
		Task<List<AsignacionCandidatoDTO>> GetAsignacionesActivasAsync(int partidoId);
		Task<bool> ValidarAsignacionCandidatoAsync(int candidatoId, int puestoElectivoId, int partidoId);
		Task<AsignacionCandidatoDTO?> GetByIdWithIncludesAsync(int id);
		Task<List<AsignacionCandidatoDTO>> GetAsignacionesConPuestosInactivosAsync();
		Task<bool> LiberarAsignacionesDePuestosInactivosAsync();
		Task<bool> DesactivarAsignacionesPorPuestoAsync(int puestoElectivoId);
	}
}