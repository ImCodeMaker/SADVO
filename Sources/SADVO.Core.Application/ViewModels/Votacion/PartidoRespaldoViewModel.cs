namespace SADVO.Core.Application.ViewModels.Votacion
{
	public class PartidoRespaldoViewModel
	{
		public int PartidoId { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string Siglas { get; set; } = string.Empty;
		public string? Color { get; set; }
	}
}
