using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.ResumenElectoral
{
	public class ResumenElectoralViewModel
	{
		[Required(ErrorMessage = "Debe seleccionar un año")]
		[Display(Name = "Año")]
		public int AñoSeleccionado { get; set; }

		public List<int> AñosDisponibles { get; set; } = new List<int>();

		public List<EleccionResumenViewModel> Elecciones { get; set; } = new List<EleccionResumenViewModel>();

		public bool MostrarResultados { get; set; } = false;
	}
}
