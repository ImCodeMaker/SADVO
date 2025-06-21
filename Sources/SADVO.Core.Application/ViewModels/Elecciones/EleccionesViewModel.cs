namespace SADVO.Core.Application.ViewModels.Elecciones
{
	public class EleccionViewModel
	{
		public int Id { get; set; }
		public required string Nombre { get; set; }
		public DateTime FechaRealizacion { get; set; }
		public bool Estado { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime FechaFinalizacion { get; set; }
		public int CantidadPartidos { get; set; }
		public int CantidadPuestos { get; set; }
		public bool EsActiva { get; set; }
		public bool EstaFinalizada => FechaFinalizacion != default(DateTime);
	}
}
