using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.ViewModels.Votacion
{
	public class ResultadoVotoViewModel
	{
		public bool Exitoso { get; set; }
		public string Mensaje { get; set; } = string.Empty;
		public List<string> Errores { get; set; } = new List<string>();
		public bool TieneErrores => Errores.Any();
		public string MensajeCompleto => TieneErrores ? string.Join("; ", Errores) : Mensaje;
		public string CssClass => Exitoso ? "alert-success" : "alert-danger";
		public string Icono => Exitoso ? "fas fa-check-circle" : "fas fa-exclamation-triangle";
	}
}
