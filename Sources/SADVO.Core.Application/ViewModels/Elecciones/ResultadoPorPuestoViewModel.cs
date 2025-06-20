using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.ViewModels.Elecciones
{
	public class ResultadoPorPuestoViewModel
	{
		public required string PuestoNombre { get; set; }
		public List<ResultadoCandidatoViewModel> Candidatos { get; set; } = new List<ResultadoCandidatoViewModel>();
	}
}
