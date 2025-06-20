namespace SADVO.Core.Application.ViewModels.Elecciones
{
	public class ResultadoCandidatoViewModel
	{
		public required string CandidatoNombre { get; set; }
		public required string PartidoPoliticoNombre { get; set; }
		public int CantidadVotos { get; set; }
		public double Porcentaje { get; set; }
		public bool EsGanador { get; set; }
	}
}
