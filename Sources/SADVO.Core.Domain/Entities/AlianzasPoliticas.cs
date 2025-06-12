using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class AlianzasPoliticas : SharedIdEntity<int>
	{
		public required int PartidoSolicitanteId { get; set; }
		public required int PartidoDestinoId { get; set; }
		public required bool Estado { get; set; } = true;
		public required DateTime FechaSolicitud {  get; set; }
		public required DateTime FechaRespuesta { get; set; }

		#region Relacion muchos a uno con Partidos Politicos 
		public required PartidosPoliticos PartidoSolicitante { get; set; }
		public required PartidosPoliticos PartidoDestino { get; set; }
		#endregion

	}
}
