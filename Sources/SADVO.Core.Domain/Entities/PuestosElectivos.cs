
using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class PuestosElectivos : SharedEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Descripcion { get; set; }

		#region Relacion uno a muchos - Un puesto Electivo puede tener Votos y Varios Candidatos
		public ICollection<AsignacionCandidatos> AsignacionesCandidatos { get; set; } = new List<AsignacionCandidatos>();
		public ICollection<Votos> Votos { get; set; } = new List<Votos>();
		#endregion
	}
}
