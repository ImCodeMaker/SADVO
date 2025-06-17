using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.Candidatos
{
	public class CrearCandidatosViewModel
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required bool Estado { get; set; } = true;
		[Display(Name = "Logo")]
		[DataType(DataType.Upload)]
		public IFormFile? LogoFile { get; set; }
	}
}
