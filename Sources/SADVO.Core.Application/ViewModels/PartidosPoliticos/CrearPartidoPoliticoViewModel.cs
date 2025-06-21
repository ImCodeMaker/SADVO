using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.PartidosPoliticos
{
	public class CrearPartidosPoliticosViewModel
	{
		[Required]
		public required string Nombre { get; set; }
		[Required]
		public required string Siglas { get; set; }
		[Required]
		public required string Descripcion { get; set; }
		public required string? Logo { get; set; }

		[Required]
		[Display(Name = "Logo")]
		[DataType(DataType.Upload)]
		public IFormFile? LogoFile { get; set; }
	}
}
