using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class Votos : SharedIdEntity<int>
	{
		public required int EleccionId { get; set; }
		public required int CiudadanoId { get; set; }
		public required int PuestoElectivoId { get; set; }
		public required int CandidatoId { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public required DateTime FechaVoto { get; set; }

		#region Relacion Muchos a uno - Varios Votos estan relacionados a una eleccion, ciudadano, puestos electivos, candidatos & etc
		public required Elecciones Eleccion { get; set; }
		public required Ciudadanos Ciudadano { get; set; }
		public required PuestosElectivos PuestoElectivo { get; set; }
		public required Candidatos Candidato { get; set; }
		public required PartidosPoliticos PartidoPolitico { get; set; }
		#endregion
	}
}
