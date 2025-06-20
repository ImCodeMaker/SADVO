using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SADVO.Core.Application.ViewModels.PartidosPoliticos;

namespace SADVO.Core.Application.ViewModels.Alianzas
{
	public class CrearAlianzaViewModel
	{
		public int PartidoSolicitanteId { get; set; }

		[Required(ErrorMessage = "Debe seleccionar un partido")]
		[Display(Name = "Partido")]
		public int PartidoDestinoId { get; set; }

		public List<PartidosPoliticosViewModel> PartidosDisponibles { get; set; } = new();
	}
}
