using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.PuestosElectivos
{
	public class UpdatePuestoElectivoViewModel
	{
		[Required]
		public required int Id { get; set; }
		[Required]
		public required string Nombre { get; set; }
		[Required]
		public required string Descripcion { get; set; }
		[Required]
		public required bool Estado { get; set; } = true;
	}
}
