
namespace SADVO.Core.Application.Dtos.Elecciones
{
	public class ResultadoEleccionDTO
	{
		public int PuestoElectivoId { get; set; }
		public required string PuestoElectivoNombre { get; set; }
		public int CandidatoId { get; set; }
		public required string CandidatoNombre { get; set; }
		public required string PartidoPoliticoNombre { get; set; }
		public int CantidadVotos { get; set; }
		public double Porcentaje { get; set; }
	}
}
