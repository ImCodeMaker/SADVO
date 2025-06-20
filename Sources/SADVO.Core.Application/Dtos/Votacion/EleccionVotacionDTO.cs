using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.Dtos.Votacion
{
	public class EleccionVotacionDTO
	{
		public int EleccionId { get; set; }
		public string NombreEleccion { get; set; } = string.Empty;
		public DateTime FechaRealizacion { get; set; }
		public List<PuestoElectivoVotacionDTO> PuestosElectivos { get; set; } = new List<PuestoElectivoVotacionDTO>();
		public bool YaVotoCompleto { get; set; }
	}
}
