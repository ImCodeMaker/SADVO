namespace SADVO.Core.Application.Dtos.AsignacionCandidatos
{
	public class CrearAsignacionCandidatoDTO
	{
		public required int CandidatoId { get; set; }
		public required int PuestoElectivoId { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public int? PartidoRespaldaId { get; set; }
		public bool EsRespaldo { get; set; }
	}
}
