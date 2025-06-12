using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Debes ingresar tu email para poder continuar.")]
		public required string Email { get; set; }
		[Required(ErrorMessage = "Debes ingresar tu contraseña para poder continuar.")]
		public required string Contraseña { get; set; }
	}
}
