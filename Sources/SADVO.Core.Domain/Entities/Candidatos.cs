using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class Candidatos : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Foto { get; set; }
		public required bool Estado { get; set; } = true;
		public required int PartidoPoliticoId { get; set; } 
		public required string PartidoName { get; set; }
		public DateTime FechaCreacion { get; set; } = DateTime.Now;

		#region Relaciones

		// Un candidato pertenece a un partido político
		public required PartidosPoliticos PartidoPolitico { get; set; }

		// Un candidato puede tener muchas asignaciones a puestos
		public ICollection<AsignacionCandidatos> Asignaciones { get; set; } = new List<AsignacionCandidatos>();

		#endregion
	}
}
