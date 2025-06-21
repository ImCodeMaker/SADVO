namespace SADVO.Core.Application.Dtos.Elecciones
{
	public class EleccionDTO
	{
		public int Id { get; set; }
		public required string Nombre { get; set; }
		public required int Año { get; set; }
		public bool Estado { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime FechaFinalizacion { get; set; }
		public int CantidadPartidos { get; set; }
		public int CantidadPuestos { get; set; }
		public bool EsActiva { get; set; }
	}
}
