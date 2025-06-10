
using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class Candidatos : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Foto { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public required bool Estado { get; set; } = true;
		public DateTime FechaCreacion { get; set; } = DateTime.Now;
		public DateTime FechaModificacion { get; set; }

		#region Relacion Muchos a Uno - Un Candidato puede pertenecer a varios particulos
		public required PartidosPoliticos PartidoPolitico { get; set; }
		#endregion

		#region Relacion uno a Muchos - Un Candidato puede tener varias Asignaciones 
		public required ICollection<AsignacionCandidatos> asignacionCandidato { get; set; } = new List<AsignacionCandidatos>();
		#endregion
	}
}
