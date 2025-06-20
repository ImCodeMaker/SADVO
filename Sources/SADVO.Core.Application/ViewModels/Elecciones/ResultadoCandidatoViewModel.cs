namespace SADVO.Core.Application.ViewModels.Elecciones
{
	public class ResultadoCandidatoViewModel
	{
		public int CandidatoId { get; set; }
		public string CandidatoNombre { get; set; } = string.Empty;
		public string PartidoPoliticoNombre { get; set; } = string.Empty;
		public int CantidadVotos { get; set; }
		public double Porcentaje { get; set; }
		public bool EsGanador { get; set; }
		public int Posicion { get; set; }

		// Propiedades para la vista
		public string PorcentajeFormateado => $"{Porcentaje:F1}%";
		public string VotosFormateados => CantidadVotos.ToString("N0");
		public string ClaseCss => EsGanador ? "ganador" : $"posicion-{Posicion}";
	}
}
