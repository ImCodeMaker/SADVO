namespace SADVO.Core.Application.Dtos
{
	public class CedulaOCRResult
	{
		public bool Exitoso { get; set; }
		public string? NumeroCedula { get; set; }
		public float Confianza { get; set; }
		public string? TextoCompleto { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
