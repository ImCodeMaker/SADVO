using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.Elecciones
{
	public class CreateEleccionViewModel
	{
		[Required(ErrorMessage = "El nombre de la elección es obligatorio")]
		[StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
		public string? Nombre { get; set; }

		[Required(ErrorMessage = "El año de la elección es obligatorio")]
		[Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
		[Display(Name = "Año")]
		public int Año { get; set; }

		public List<string> ErroresValidacion { get; set; } = new List<string>();
	}
}