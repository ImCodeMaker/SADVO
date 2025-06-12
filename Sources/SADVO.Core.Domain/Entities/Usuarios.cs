using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class Usuarios : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required string Email { get; set; }
		public required string NombreUsuario { get; set; }
		public required string Contraseña { get; set; }
		public required string Rol { get; set; }
		public required bool Estado { get; set; } = true;

		#region Un usuario tiene puede ser un dirigente
		public required AsignacionDirigentes AsignacionDirigente { get; set; }
		#endregion

	}
}
