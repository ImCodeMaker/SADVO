using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.ViewModels.Votacion
{
	public class PuestoElectivoVotacionViewModel
	{
		public int PuestoElectivoId { get; set; }
		public string NombrePuesto { get; set; } = string.Empty;
		public List<CandidatoVotacionViewModel> Candidatos { get; set; } = new List<CandidatoVotacionViewModel>();
		public bool YaVotado { get; set; }
		public string EstadoVotacion => YaVotado ? "Completado" : "Pendiente";
		public string CssClassEstado => YaVotado ? "text-success" : "text-warning";
		public string IconoEstado => YaVotado ? "fas fa-check-circle" : "fas fa-clock";
	}
}
