using SADVO.Core.Domain.Common;
using System;

namespace SADVO.Core.Domain.Entities
{
	public class AsignacionDirigentes : SharedIdEntity<int>
	{
		// Propiedades básicas
		public required int UsuarioId { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public required bool Estado { get; set; } = true;
		public required string UsuarioName { get; set;}
		public DateTime FechaAsignacion { get; set; } = DateTime.Now;
		public required Usuarios Usuario { get; set; }
		public required PartidosPoliticos PartidoPolitico { get; set; }
	}
}