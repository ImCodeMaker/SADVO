using System.Collections.Generic;
using SADVO.Core.Application.Dtos.Alianzas;

namespace SADVO.Core.Application.ViewModels.Alianzas
{
	public class AlianzasViewModel
	{
		public List<AlianzasDTO> SolicitudesPendientes { get; set; } = new();
		public List<AlianzasDTO> SolicitudesEnviadas { get; set; } = new();
		public List<AlianzasDTO> AlianzasActivas { get; set; } = new();
	}
}
