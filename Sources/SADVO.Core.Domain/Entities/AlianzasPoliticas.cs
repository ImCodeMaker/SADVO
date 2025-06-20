using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public enum EstadoAlianza
	{
		EnEspera = 0,
		Aceptada = 1,
		Rechazada = 2,
		Rota = 3
	}

	public class AlianzasPoliticas : SharedIdEntity<int>
	{
		public required int PartidoSolicitanteId { get; set; }
		public required int PartidoDestinoId { get; set; }
		public required bool Estado { get; set; } = true;
		public required EstadoAlianza EstadoAlianza { get; set; }
		public required DateTime FechaSolicitud {  get; set; }
		public required DateTime FechaRespuesta { get; set; }

		#region Relacion muchos a uno con Partidos Politicos 
		public required PartidosPoliticos PartidoSolicitante { get; set; }
		public required PartidosPoliticos PartidoDestino { get; set; }
		#endregion

	}
}
