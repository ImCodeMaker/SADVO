using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.ViewModels.Elecciones
{
	public class ResultadosEleccionViewModel
	{
		public int EleccionId { get; set; }
		public string EleccionNombre { get; set; }
		public List<ResultadoPorPuestoViewModel> ResultadosPorPuesto { get; set; } = new List<ResultadoPorPuestoViewModel>();
	}
}
