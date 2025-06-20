namespace SADVO.Core.Application.Dtos.AsignacionCandidatos
{
	public class AsignacionCandidatoDTO
	{
		public required int Id { get; set; }
		public required int CandidatoId { get; set; }
		public required int PuestoElectivoId { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public required bool Estado { get; set; }
		public int? PartidoRespaldaId { get; set; }
		public DateTime FechaAsignacion { get; set; }

		// Propiedades navegacionales
		public string NombreCandidato { get; set; } = string.Empty;
		public string ApellidoCandidato { get; set; } = string.Empty;
		public string NombreCompleto => $"{NombreCandidato} {ApellidoCandidato}";
		public string NombrePuestoElectivo { get; set; } = string.Empty;
		public string NombrePartidoPolitico { get; set; } = string.Empty;
		public string SiglasPartidoPolitico { get; set; } = string.Empty;
		public string PartidoConSiglas => $"{NombrePartidoPolitico} ({SiglasPartidoPolitico})";

		public string? NombrePartidoQueRespalda { get; set; }
		public string? SiglasPartidoQueRespalda { get; set; }
		public string? PartidoRespaldanteConSiglas =>
			!string.IsNullOrEmpty(NombrePartidoQueRespalda) ?
			$"{NombrePartidoQueRespalda} ({SiglasPartidoQueRespalda})" : null;
	}
}
