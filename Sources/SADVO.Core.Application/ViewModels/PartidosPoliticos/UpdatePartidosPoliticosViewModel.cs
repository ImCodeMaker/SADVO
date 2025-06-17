using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.PartidosPoliticos
{
	public class UpdatePartidosPoliticosViewModel
	{
		public required int Id { get; set; }
		[Required]
		public required string Nombre { get; set; }
		[Required]
		public required string Siglas { get; set; }
		[Required]
		public required string Descripcion { get; set; }
		// Ruta o URL al logo guardado
		public required string? Logo { get; set; }

		// Archivo nuevo subido
		[Display(Name = "Logo")]
		[DataType(DataType.Upload)]
		public IFormFile? LogoFile { get; set; }

		public required bool Estado { get; set; } = true;
	}
}