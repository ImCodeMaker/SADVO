﻿using SADVO.Core.Application.Dtos.Common;

namespace SADVO.Core.Application.Dtos.PuestoElectivo
{
	public class PuestoElectivoDTO : SharedEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Descripcion { get; set; }
	}
}
