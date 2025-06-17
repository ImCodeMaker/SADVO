
using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class Candidatos : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Foto { get; set; }
		public required bool Estado { get; set; } = true;
		public DateTime FechaCreacion { get; set; } = DateTime.Now;

		#region Relacion uno a Muchos - Un Candidato puede tener varias Asignaciones 
		public required ICollection<AsignacionCandidatos> asignacionCandidato { get; set; } = new List<AsignacionCandidatos>();
		#endregion
	}
}
