using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class Elecciones : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required bool Estado { get; set; } = true;
		public required int Año { get; set; }
		public DateTime FechaCreacion { get; set; }

		#region Relacion uno a muchos  - Una eleccion Puede tener varios historiales y votos
		public ICollection<HistorialVotaciones> HistorialVotaciones { get; set; } = new List<HistorialVotaciones>();
		public ICollection<Votos> Votos { get; set; } = new List<Votos>();
		#endregion
	}
}
