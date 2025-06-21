namespace SADVO.Core.Application.ViewModels.Elecciones
{
	public class ResultadoPorPuestoViewModel
	{
		public int PuestoElectivoId { get; set; }
		public string PuestoNombre { get; set; } = string.Empty;
		public int TotalVotos { get; set; }
		public List<ResultadoCandidatoViewModel> Candidatos { get; set; } = new List<ResultadoCandidatoViewModel>();

		// Propiedades calculadas
		public int TotalCandidatos => Candidatos.Count;
		public ResultadoCandidatoViewModel? Ganador => Candidatos.FirstOrDefault(c => c.EsGanador);
	}
}
