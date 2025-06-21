namespace SADVO.Core.Application.Dtos.Votacion
{
	public class PartidoRespaldoDTO
	{
		public int PartidoId { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string Siglas { get; set; } = string.Empty;
		public string? Color { get; set; }
	}
}
