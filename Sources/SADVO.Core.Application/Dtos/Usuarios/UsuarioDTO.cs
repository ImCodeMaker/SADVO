﻿using SADVO.Core.Application.Dtos.Common;

namespace SADVO.Core.Application.Dtos.Usuarios
{
	public class UsuarioDTO : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Email { get; set; }
		public required string NombreUsuario { get; set; }
		public required string Contraseña { get; set; }
		public required string Rol { get; set; }
		public required bool Estado { get; set; }
	}
}
