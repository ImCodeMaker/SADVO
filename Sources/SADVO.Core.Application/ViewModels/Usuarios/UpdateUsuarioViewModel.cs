﻿using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.Usuarios
{
	public class UpdateUsuarioViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio.")]
		[StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
		public required string Nombre { get; set; }

		[Required(ErrorMessage = "El apellido es obligatorio.")]
		[StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
		public required string Apellido { get; set; }

		[Required(ErrorMessage = "El email es obligatorio.")]
		[EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
		public required string Email { get; set; }

		[Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
		[StringLength(30, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 4 y 30 caracteres.")]
		public required string NombreUsuario { get; set; }

		[StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
		[DataType(DataType.Password)]
		public string? Contraseña { get; set; }

		[Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
		[DataType(DataType.Password)]
		public string? ConfirmarContraseña { get; set; }

		public string? Rol { get; set; } = "Dirigente";

		public required bool Estado { get; set; } = true;
	}
}
