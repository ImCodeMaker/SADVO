using SADVO.Core.Application.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.PuestosElectivos
{
	public class PuestoElectivoViewModel : SharedEntity<int>
	{
		[Required(ErrorMessage = "Este campo es requerido.")]
		[DataType(DataType.Text)]
		public required string Nombre { get; set; }
		[Required(ErrorMessage = "Este campo es requerido.")]
		[DataType(DataType.Text)]
		public required string Descripcion { get; set; }
	}
}
