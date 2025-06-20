namespace SADVO.Core.Application.ViewModels.Alianzas
{
	public class ConfirmacionViewModel
	{
		public int Id { get; set; }
		public string Mensaje { get; set; } = string.Empty;
		public string Accion { get; set; } = string.Empty;
		public string TituloBoton { get; set; } = string.Empty;
	}
}