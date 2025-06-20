using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Domain.Entities
{
	public class ResultadoEleccion
	{
		public required int PuestoElectivoId { get; set; }
		public required string PuestoElectivoNombre { get; set; }
		public required int CandidatoId { get; set; }
		public required string CandidatoNombre { get; set; }
		public required string PartidoPoliticoNombre { get; set; }
		public  int CantidadVotos { get; set; }
		public  double Porcentaje { get; set; }
	}
}
