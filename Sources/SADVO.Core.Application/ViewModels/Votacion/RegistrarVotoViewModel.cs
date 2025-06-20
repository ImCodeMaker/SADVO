using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.Votacion
{
	public class RegistrarVotoViewModel
	{
		[Required]
		public int EleccionId { get; set; }

		[Required]
		public int CiudadanoId { get; set; }

		[Required]
		public int PuestoElectivoId { get; set; }

		[Required]
		public int CandidatoId { get; set; }

		[Required]
		public int PartidoPoliticoId { get; set; }
	}

}
