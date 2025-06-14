using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.Usuarios
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Debes ingresar tu nombre de usuario para poder continuar.")]
		public required string NombreUsuario { get; set; }
		[Required(ErrorMessage = "Debes ingresar tu contraseña para poder continuar.")]
		public required string Contraseña { get; set; }
	}
}
