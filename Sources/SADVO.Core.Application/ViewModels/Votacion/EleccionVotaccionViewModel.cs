namespace SADVO.Core.Application.ViewModels.Votacion
{
	public class EleccionVotacionViewModel
	{
		public int EleccionId { get; set; }
		public string NombreEleccion { get; set; } = string.Empty;
		public DateTime FechaRealizacion { get; set; }
		public string FechaFormateada => FechaRealizacion.ToString("dd/MM/yyyy");
		public List<PuestoElectivoVotacionViewModel> PuestosElectivos { get; set; } = new List<PuestoElectivoVotacionViewModel>();
		public bool YaVotoCompleto { get; set; }
		public int TotalPuestos => PuestosElectivos.Count;
		public int PuestosVotados => PuestosElectivos.Count(p => p.YaVotado);
		public double PorcentajeCompletado => TotalPuestos > 0 ? (double)PuestosVotados / TotalPuestos * 100 : 0;
		public string EstadoGeneral => YaVotoCompleto ? "Votación Completada" : $"Progreso: {PuestosVotados}/{TotalPuestos}";
	}
}
