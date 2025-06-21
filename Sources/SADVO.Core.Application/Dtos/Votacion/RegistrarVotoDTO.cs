namespace SADVO.Core.Application.Dtos.Votacion
{
	public class RegistrarVotoDTO
	{
		public int EleccionId { get; set; }
		public int CiudadanoId { get; set; }
		public int PuestoElectivoId { get; set; }
		public int CandidatoId { get; set; }
		public int PartidoPoliticoId { get; set; }
	}
}
