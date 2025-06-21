using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.Dtos.ResumenElectoral
{
	public class ResumenElectoralRequestDTO
	{
		[Required(ErrorMessage = "El año es requerido")]
		[Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
		public int Año { get; set; }
	}
}
