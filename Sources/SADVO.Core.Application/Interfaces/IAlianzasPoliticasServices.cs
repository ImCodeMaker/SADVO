using SADVO.Core.Application.Dtos.Alianzas;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IAlianzasPoliticasServices :
		IGenericServices<CrearAlianzaDTO, UpdateAlianzaDTO, AlianzasDTO, AlianzasPoliticas>
	{
		Task<List<AlianzasDTO>> GetSolicitudesPendientesAsync(int partidoId);
		Task<List<AlianzasDTO>> GetSolicitudesEnviadasAsync(int partidoId);
		Task<List<AlianzasDTO>> GetAlianzasActivasAsync(int partidoId);
		Task<bool> AceptarSolicitudAsync(int id);
		Task<bool> RechazarSolicitudAsync(int id);
		Task<bool> ExisteRelacionPendienteAsync(int solicitanteId, int destinoId);
		Task<bool> EliminarSolicitudAsync(int id);

	}
}
