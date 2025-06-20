using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.Dtos.Elecciones
{
	public class CrearEleccionDTO
	{
		[Required(ErrorMessage = "El nombre de la elección es requerido")]
		[StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
		public string Nombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "El año es requerido")]
		[Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
		public int Año { get; set; }
	}
}
