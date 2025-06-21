namespace SADVO.Core.Application.Dtos.ResumenElectoral
{
	public class ResumenElectoralDTO
	{
		public int EleccionId { get; set; }
		public string NombreEleccion { get; set; } = string.Empty;
		public DateTime FechaRealizacion { get; set; }
		public int CantidadPartidos { get; set; }
		public int CantidadCandidatos { get; set; }
		public int TotalVotos { get; set; }
		public bool Estado { get; set; }
	}
}
