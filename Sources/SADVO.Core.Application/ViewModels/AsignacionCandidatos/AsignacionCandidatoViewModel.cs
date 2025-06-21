namespace SADVO.Core.Application.ViewModels.AsignacionCandidatos
{
	public class AsignacionCandidatoViewModel
	{
		public int Id { get; set; }
		public int CandidatoId { get; set; }
		public int PuestoElectivoId { get; set; }
		public int PartidoPoliticoId { get; set; }
		public int? PartidoRespaldaId { get; set; }
		public bool Estado { get; set; }
		public DateTime FechaAsignacion { get; set; }

		public string NombreCandidato { get; set; } = string.Empty;
		public string ApellidoCandidato { get; set; } = string.Empty;
		public string NombreCompleto => $"{NombreCandidato} {ApellidoCandidato}";
		public string NombrePuestoElectivo { get; set; } = string.Empty;
		public string NombrePartidoPolitico { get; set; } = string.Empty;
		public string SiglasPartidoPolitico { get; set; } = string.Empty;
		public string PartidoConSiglas => $"{NombrePartidoPolitico} ({SiglasPartidoPolitico})";

		// NUEVO: lista de partidos que respaldan al candidato
		public List<string> PartidosQueRespaldan { get; set; } = new();
	}

}
