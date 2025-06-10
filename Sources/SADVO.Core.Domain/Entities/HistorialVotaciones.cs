using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class HistorialVotaciones : SharedIdEntity<int>
	{
		public required int EleccionId { get; set; }
		public required int CiudadanoId { get; set; }
		public required bool HaVotado {  get; set; }
		public required DateTime FechaVotacion { get; set; }
		public required bool EmailEnviado { get; set; }

		#region Relacion muchos a uno - Un Historial puede tener varias Elecciones y varios Ciudadanos
		public required Elecciones Eleccion { get; set;  }
		public required Ciudadanos Ciudadano { get; set; }
		#endregion 
	}
}
