namespace SADVO.Core.Application.ViewModels.Dirigente
{
	public class DashboardDirigenteViewModel
	{
		public string PartidoPoliticoNombre { get; set; } = string.Empty;
		public string PartidoPoliticoSiglas { get; set; } = string.Empty;
		public int PartidoPoliticoId { get; set; }
		public int TotalCandidatos { get; set; }
		public int CandidatosAsignados { get; set; }
		public int TotalPuestosDisponibles { get; set; }
		public int PropuestasPendientes { get; set; }
		public double PorcentajeAsignacion => TotalPuestosDisponibles > 0
			? Math.Round((double)CandidatosAsignados / TotalPuestosDisponibles * 100, 1)
			: 0;
	}
}