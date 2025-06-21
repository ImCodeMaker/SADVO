using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Dtos.Alianzas
{
	public class AlianzasDTO
	{
		public int Id { get; set; }
		public int PartidoSolicitanteId { get; set; }
		public int PartidoDestinoId { get; set; }
		public bool Estado { get; set; }
		public EstadoAlianza EstadoAlianza { get; set; }
		public DateTime FechaSolicitud { get; set; }
		public DateTime? FechaRespuesta { get; set; }
		public string NombrePartidoSolicitante { get; set; } = string.Empty;
		public string SiglasPartidoSolicitante { get; set; } = string.Empty;
		public string NombrePartidoDestino { get; set; } = string.Empty;
		public string SiglasPartidoDestino { get; set; } = string.Empty;

		public string EstadoTexto => EstadoAlianza switch
		{
			EstadoAlianza.EnEspera => "En espera de respuesta",
			EstadoAlianza.Aceptada => "Aceptada",
			EstadoAlianza.Rechazada => "Rechazada",
			_ => "Desconocido"
		};

		public string NombrePartidoConSiglas(bool esSolicitante) =>
			esSolicitante
				? $"{NombrePartidoSolicitante} ({SiglasPartidoSolicitante})"
				: $"{NombrePartidoDestino} ({SiglasPartidoDestino})";
	}
}

