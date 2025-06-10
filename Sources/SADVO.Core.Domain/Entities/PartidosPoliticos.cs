
using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class PartidosPoliticos : SharedEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Descripcion { get; set; }
		public required string Siglas { get; set; }
		public required string Logo { get; set;}
		public required ICollection<AlianzasPoliticas> AlianzasPoliticas { get; set; }
		public required ICollection<AsignacionCandidatos> AsignacionCandidatos { get; set; }

		#region Relacion de uno a muchos - Un Partido politico puede tener muchas alianzas, varios candidatos y varios Dirigentes
		public ICollection<AlianzasPoliticas> AlianzasSolicitadas { get; set; } = new List<AlianzasPoliticas>();
		public ICollection<AlianzasPoliticas> AlianzasRecibidas { get; set; } = new List<AlianzasPoliticas>();
		public ICollection<AsignacionCandidatos> AsignacionesCandidatos { get; set; } = new List<AsignacionCandidatos>();
		public ICollection<Candidatos> Candidatos { get; set; } = new List<Candidatos>();
		public ICollection<AsignacionDirigentes> Dirigentes { get; set; } = new List<AsignacionDirigentes>();
		#endregion
	}
}
