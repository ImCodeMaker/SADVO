using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.PuestosElectivos
{
	public class CrearPuestoElectivoViewModel
	{
		[Required(ErrorMessage = "Este campo es requerido.")]
		public required string Nombre { get; set; }
		[Required(ErrorMessage = "Este campo es requerido.")]
		public required string Descripcion { get; set; }
	}
}
