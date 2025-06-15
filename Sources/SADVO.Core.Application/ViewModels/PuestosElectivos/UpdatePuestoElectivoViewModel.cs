using SADVO.Core.Application.Dtos.Common;

namespace SADVO.Core.Application.ViewModels.PuestosElectivos
{
	public class UpdatePuestoElectivoViewModel
	{
		public required int Id { get; set; }
		public required string Nombre { get; set; }
		public required string Descripcion { get; set; }
		public required bool Estado { get; set; } = true;
	}
}
