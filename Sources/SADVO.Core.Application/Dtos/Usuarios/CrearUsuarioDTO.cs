using SADVO.Core.Application.Dtos.Common;

namespace SADVO.Core.Application.Dtos.Usuarios
{
	public class CrearUsuarioDTO : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Email { get; set; }
		public required string NombreUsuario { get; set; }
		public required string Contraseña { get; set; }
		public string? Rol { get; set; }
		public bool Estado { get; set; } = true;
	}
}
