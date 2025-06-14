using SADVO.Core.Application.ViewModels.Common;

namespace SADVO.Core.Application.ViewModels.Usuarios
{
	public class CreateUserViewModel : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Email { get; set; }
		public required string NombreUsuario { get; set; }
		public required string Contraseña { get; set; }
		public required string Rol { get; set; }
		public required string Estado { get; set; } = "Activo";
	}
}
