namespace SADVO.Core.Application.ViewModels.AsignacionDirigentes
{
	public class AsignacionDirigentesViewModel 
	{
		public int Id { get; set; }
		public int UsuarioId { get; set; }
		public int PartidoPoliticoId { get; set; }
		public bool Estado { get; set; } = true;
		public string? UsuarioName { get; set; }
	}
}
