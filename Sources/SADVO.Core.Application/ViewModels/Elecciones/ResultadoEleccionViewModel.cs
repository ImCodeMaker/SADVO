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
		public string EleccionNombre { get; set; } = string.Empty;
		public int EleccionAnio { get; set; }
		public DateTime FechaFinalizacion { get; set; }

		public List<ResultadoPorPuestoViewModel> ResultadosPorPuesto { get; set; } = new List<ResultadoPorPuestoViewModel>();

		// Estadísticas generales
		public int TotalPuestosDisputados { get; set; }
		public int TotalCandidatos { get; set; }
		public int TotalVotosEmitidos { get; set; }

		// Propiedades calculadas
		public bool TieneResultados => ResultadosPorPuesto.Any() && TotalVotosEmitidos > 0;
	}
}
