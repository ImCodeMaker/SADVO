using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.Dtos.Votacion
{
	public class PuestoElectivoVotacionDTO
	{
		public int PuestoElectivoId { get; set; }
		public string NombrePuesto { get; set; } = string.Empty;
		public List<CandidatoVotacionDTO> Candidatos { get; set; } = new List<CandidatoVotacionDTO>();
		public bool YaVotado { get; set; }
	}
}
