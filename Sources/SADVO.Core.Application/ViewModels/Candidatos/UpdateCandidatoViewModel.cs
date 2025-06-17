using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels.Candidatos
{
	public class UpdateCandidatoViewModel : SharedIdEntity<int>
	{
		public required string Nombre { get; set; }
		public required string Apellido { get; set; }
		public required bool Estado { get; set; } = true;
		[Display(Name = "Logo")]
		[DataType(DataType.Upload)]
		public IFormFile? LogoFile { get; set; }
	}
}

