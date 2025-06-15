using SADVO.Core.Application.Dtos.Common;

namespace SADVO.Core.Application.Dtos.PartidosPoliticos
{
	public class UpdatePartidoPoliticoDTO : SharedEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Descripcion { get; set; }
		public required string Siglas { get; set; }
		public required string Logo { get; set; }
	}
}
