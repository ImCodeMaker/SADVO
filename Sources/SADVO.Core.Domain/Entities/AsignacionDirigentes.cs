using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class AsignacionDirigentes : SharedIdEntity<int>
	{
		public required int UsuarioId { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public required PartidosPoliticos PartidosPoliticos { get; set; }
		public DateTime FechaAsignacion { get; set; } = DateTime.Now;

		#region Relacion de uno a uno - Un Dirigente puede ser un Usuario
		public required Usuarios Usuario { get; set; }
		#endregion

		#region Relacion de muchos a uno - Un Dirigente puede ser dirigente de varios partidos 
		public required PartidosPoliticos partidosPoliticos { get; set; }

		#endregion
	}
}
