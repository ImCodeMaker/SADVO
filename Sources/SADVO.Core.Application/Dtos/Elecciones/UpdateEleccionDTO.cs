using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.Dtos.Elecciones
{
	public class UpdateEleccionDTO
	{
		public required string Nombre { get; set; }
		public required DateTime FechaRealizacion { get; set; }
		public required bool Estado { get; set; }
	}
}
