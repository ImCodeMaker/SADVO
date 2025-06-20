using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SADVO.Core.Application.ViewModels
{
	public class CedulaScanViewModel
	{
		[Display(Name = "Imagen de Cédula")]
		[DataType(DataType.Upload)]
		[Required(ErrorMessage = "Debe seleccionar una imagen")]
		public IFormFile? CedulaImageFile { get; set; }

		[Display(Name = "Número de Cédula Extraído")]
		public string? NumeroCedula { get; set; }

		[Display(Name = "Confianza")]
		public float Confianza { get; set; }

		public bool Procesado { get; set; }
		public string? ErrorMessage { get; set; }
	}
}