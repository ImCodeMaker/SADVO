using SADVO.Core.Application.ViewModels.Candidatos;
using SADVO.Core.Application.ViewModels.PuestosElectivos;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.AsignacionCandidatos
{
	public class CrearAsignacionCandidatoViewModel
	{
		[Required(ErrorMessage = "Debe seleccionar un candidato")]
		[Display(Name = "Candidato")]
		public int CandidatoId { get; set; }

		[Required(ErrorMessage = "Debe seleccionar un puesto electivo")]
		[Display(Name = "Puesto Electivo")]
		public int PuestoElectivoId { get; set; }
		public int PartidoPoliticoId { get; set; }
		public int? PartidoRespaldaId { get; set; }
		public bool EsRespaldo { get; set; }

		// Listas para los selects
		public List<CandidatosViewModel> CandidatosDisponibles { get; set; } = new List<CandidatosViewModel>();
		public List<PuestoElectivoViewModel> PuestosDisponibles { get; set; } = new List<PuestoElectivoViewModel>();
	}
}
