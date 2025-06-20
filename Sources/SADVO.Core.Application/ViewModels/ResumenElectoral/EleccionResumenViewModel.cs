using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.ViewModels.ResumenElectoral
{
	public class EleccionResumenViewModel
	{
		public int EleccionId { get; set; }

		[Display(Name = "Nombre de la Elección")]
		public string NombreEleccion { get; set; } = string.Empty;

		[Display(Name = "Fecha de Realización")]
		[DataType(DataType.Date)]
		public DateTime FechaRealizacion { get; set; }

		[Display(Name = "Cantidad de Partidos")]
		public int CantidadPartidos { get; set; }

		[Display(Name = "Cantidad de Candidatos")]
		public int CantidadCandidatos { get; set; }

		[Display(Name = "Total de Votos")]
		public int TotalVotos { get; set; }

		public bool Estado { get; set; }

		[Display(Name = "Fecha de Realización")]
		public string FechaRealizacionFormateada => FechaRealizacion.ToString("dd/MM/yyyy");
	}
}
