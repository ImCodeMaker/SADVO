namespace SADVO.Core.Application.Dtos.Candidatos
{
	public class CrearCandidatosDTO
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Foto { get; set; }
		public required bool Estado { get; set; } = true;
		public required string PartidoName { get; set; }
	}
}
