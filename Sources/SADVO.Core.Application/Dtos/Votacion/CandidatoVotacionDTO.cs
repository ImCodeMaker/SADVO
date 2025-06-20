
namespace SADVO.Core.Application.Dtos.Votacion
{
	public class CandidatoVotacionDTO
	{
		public int CandidatoId { get; set; }
		public string NombreCandidato { get; set; } = string.Empty;
		public string ApellidoCandidato { get; set; } = string.Empty;
		public string NombreCompleto => $"{NombreCandidato} {ApellidoCandidato}";
		public string? FotoCandidato { get; set; }
		public int PuestoElectivoId { get; set; }
		public int PartidoPoliticoId { get; set; }
		public string NombrePuestoElectivo { get; set; } = string.Empty;
		public int PartidoPrincipalId { get; set; }
		public string NombrePartidoPrincipal { get; set; } = string.Empty;
		public string SiglasPartidoPrincipal { get; set; } = string.Empty;
		public List<PartidoRespaldoDTO> PartidosRespaldo { get; set; } = new List<PartidoRespaldoDTO>();
		public bool TieneRespaldos => PartidosRespaldo.Any();
	}
}
