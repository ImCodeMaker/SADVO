using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.ViewModels.Votacion
{
	public class CandidatoVotacionViewModel
	{
		public int CandidatoId { get; set; }
		public string NombreCandidato { get; set; } = string.Empty;
		public string ApellidoCandidato { get; set; } = string.Empty;
		public string NombreCompleto => $"{NombreCandidato} {ApellidoCandidato}";
		public string? FotoCandidato { get; set; }
		public string FotoUrl => !string.IsNullOrEmpty(FotoCandidato) ? FotoCandidato : "/images/default-avatar.png";
		public int PartidoPoliticoId { get; set; }
		public int PuestoElectivoId { get; set; }
		public string NombrePuestoElectivo { get; set; } = string.Empty;
		public int PartidoPrincipalId { get; set; }

		public string NombrePartidoPrincipal { get; set; } = string.Empty;
		public string SiglasPartidoPrincipal { get; set; } = string.Empty;
		public List<PartidoRespaldoViewModel> PartidosRespaldo { get; set; } = new List<PartidoRespaldoViewModel>();
		public bool TieneRespaldos => PartidosRespaldo.Any();
		public string PartidosRespaldoTexto => TieneRespaldos
			? string.Join(", ", PartidosRespaldo.Select(p => p.Siglas))
			: string.Empty;
	}

}
