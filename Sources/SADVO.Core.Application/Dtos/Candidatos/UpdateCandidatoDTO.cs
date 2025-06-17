using SADVO.Core.Application.Dtos.Common;

namespace SADVO.Core.Application.Dtos.Candidatos
{
	public class UpdateCandidatoDTO : SharedIdEntity<int>
	{
			public required string Nombre { get; set; }
			public required string Apellido { get; set; }
			public required string Foto { get; set; }
			public required bool Estado { get; set; } = true;
		}
	}

