using SADVO.Core.Application.Dtos.PartidosPoliticos;

public interface IAlianzasHelperService
{
	Task<List<PartidosPoliticosDTO>> GetPartidosDisponiblesParaAlianzaAsync(int partidoId);
	Task<bool> RomperAlianzaAsync(int alianzaId);
}
