namespace SADVO.Core.Application.Dtos.AsignacionDirigentes
{
	public class CreateAsignacionDirigentesDTO
	{
		public required int UsuarioId { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public required bool Estado { get; set; } = true;
		public required string UsuarioName { get; set; }
	}
}
