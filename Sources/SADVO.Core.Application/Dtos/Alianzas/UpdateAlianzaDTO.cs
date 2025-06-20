using SADVO.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.Dtos.Alianzas
{
	public class UpdateAlianzaDTO
	{
		public int Id { get; set; }
		public int PartidoSolicitanteId { get; set; }
		public int PartidoDestinoId { get; set; }
		public bool Estado { get; set; }
		public EstadoAlianza EstadoAlianza { get; set; }
		public DateTime FechaSolicitud { get; set; }
		public DateTime? FechaRespuesta { get; set; }
	}
}
