using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.ViewModels.AsignacionCandidatos
{
	public class AsignacionCandidatosViewModel
	{
		public List<AsignacionCandidatoViewModel> Asignaciones { get; set; } = new List<AsignacionCandidatoViewModel>();
		public bool PuedeAgregar { get; set; } = true;
	}
}
