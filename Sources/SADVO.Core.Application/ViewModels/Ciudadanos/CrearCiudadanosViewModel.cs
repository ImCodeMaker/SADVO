using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.Ciudadanos
{
	public class CrearCiudadanosViewModel
	{
		[Required(ErrorMessage = "Este campo es requerido.")]
		[DataType(DataType.Text)]
		public required string NombreCompleto { get; set; }
		[Required(ErrorMessage = "Este campo es requerido.")]
		[DataType(DataType.EmailAddress)]
		public required string Email { get; set; }
		[Required(ErrorMessage = "Este campo es requerido")]
		[MinLength(13, ErrorMessage = "El documento de identidad debe tener al menos 11 caracteres.")]
		[MaxLength(13, ErrorMessage = "El documento de identidad debe tener al menos 11 caracteres.")]
		public required string Documento_Identidad { get; set; }
	}
}
