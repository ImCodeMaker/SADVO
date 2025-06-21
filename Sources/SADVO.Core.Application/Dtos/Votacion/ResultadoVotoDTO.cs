namespace SADVO.Core.Application.Dtos.Votacion
{
	public class ResultadoVotoDTO
	{
		public bool Exitoso { get; set; }
		public string Mensaje { get; set; } = string.Empty;
		public List<string> Errores { get; set; } = new List<string>();
	}
}
