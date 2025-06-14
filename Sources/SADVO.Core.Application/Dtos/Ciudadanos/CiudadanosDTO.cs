using SADVO.Core.Application.Dtos.Common;

namespace SADVO.Core.Application.Dtos.Ciudadanos
{
	public class CiudadanosDTO : SharedEntity<int>
	{
		public required string NombreCompleto { get; set; }
		public required string Email { get; set; }
		public required string Documento_Identidad { get; set; }
	}
}
