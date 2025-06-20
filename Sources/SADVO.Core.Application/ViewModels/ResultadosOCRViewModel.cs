using System.ComponentModel.DataAnnotations;

namespace SADVO.Core.Application.ViewModels
{
	public class ResultadosOCRViewModel
	{
		public bool Exitoso { get; set; }

		[Display(Name = "Número de Cédula Extraído")]
		public string NumeroCedula { get; set; } = string.Empty;

		[Display(Name = "Nivel de Confianza")]
		public double Confianza { get; set; }

		[Display(Name = "Texto Completo Extraído")]
		public string TextoCompleto { get; set; } = string.Empty;

		[Display(Name = "Mensaje de Error")]
		public string ErrorMessage { get; set; } = string.Empty;

		[Display(Name = "Cédula Confirmada")]
		[Required(ErrorMessage = "Debes confirmar el número de cédula")]
		[RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "El formato debe ser: 000-0000000-0")]
		public string CedulaConfirmada { get; set; } = string.Empty;

		// Propiedades calculadas
		public string ConfianzaPorcentaje => $"{Confianza:F1}%";

		public string EstadoConfianza => Confianza switch
		{
			>= 90 => "Muy Alta",
			>= 75 => "Alta",
			>= 60 => "Media",
			>= 40 => "Baja",
			_ => "Muy Baja"
		};

		public string ClaseEstado => Confianza switch
		{
			>= 75 => "success",
			>= 60 => "warning",
			_ => "danger"
		};
	}
}