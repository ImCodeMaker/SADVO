using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.Dtos.AsignacionCandidatos
{
	public class UpdateAsignacionCandidatoDTO
	{
		public required int CandidatoId { get; set; }
		public required int PuestoElectivoId { get; set; }
		public required int PartidoPoliticoId { get; set; }
		public required bool Estado { get; set; } = true;
	}
}
