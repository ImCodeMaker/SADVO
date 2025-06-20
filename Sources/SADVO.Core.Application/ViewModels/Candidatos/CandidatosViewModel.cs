using SADVO.Core.Application.ViewModels.Common;

namespace SADVO.Core.Application.ViewModels.Candidatos
{
	public class CandidatosViewModel : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Foto { get; set; }
		public required bool Estado { get; set; } = true;
		public required int PartidoPoliticoId { get; set; }
		public required string PartidoName { get; set; }
		public bool TieneAsignacion { get; set; }
	}
}
