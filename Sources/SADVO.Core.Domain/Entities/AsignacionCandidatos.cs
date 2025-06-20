using SADVO.Core.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADVO.Core.Domain.Entities
{
	public class AsignacionCandidatos : SharedIdEntity<int>
	{
		public required int CandidatoId { get; set; }
		public required int PuestoElectivoId {  get; set; }
		public required int PartidoPoliticoId { get; set; }
		public int? PartidoRespaldaId { get; set; }
		public required bool Estado { get; set; } = true;
		public DateTime FechaAsignacion { get; set; } = DateTime.Now;

		#region Relacion muchos a uno con Candidatos, Puestos Electivos, Partidos Politicos 
		public required Candidatos Candidato { get; set; }
		public required PuestosElectivos puestosElectivos { get; set; }
		public required PartidosPoliticos PartidosPoliticos { get; set; }
		[ForeignKey("PartidoRespaldaId")]
		public virtual PartidosPoliticos? PartidoQueRespalda { get; set; }
		#endregion
	}
}
