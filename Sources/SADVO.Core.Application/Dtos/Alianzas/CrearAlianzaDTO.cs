using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Dtos.Alianzas
{
	public class CrearAlianzaDTO
	{
		public int PartidoSolicitanteId { get; set; }
		public int PartidoDestinoId { get; set; }
		public bool Estado { get; set; } = true;
		public EstadoAlianza EstadoAlianza { get; set; } = EstadoAlianza.EnEspera;
		public DateTime FechaSolicitud { get; set; } = DateTime.Now;
		public DateTime? FechaRespuesta { get; set; }
	}
}
