using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using Tesseract;
using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.Dtos;

namespace SADVO.Core.Application.Services
{
	public static class CedulaOCRService
	{
		private static readonly string _tessDataPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");

		public static async Task<CedulaOCRResult> ExtraerCedulaAsync(IFormFile imageFile)
		{
			try
			{
				using (var memoryStream = new MemoryStream())
				{
					await imageFile.CopyToAsync(memoryStream);
					return await Task.Run(() => ProcesarImagen(memoryStream.ToArray()));
				}
			}
			catch (Exception ex)
			{
				return new CedulaOCRResult
				{
					Exitoso = false,
					ErrorMessage = $"Error procesando imagen: {ex.Message}"
				};
			}
		}

		public static async Task<CedulaOCRResult> ExtraerCedulaAsync(string imagePath)
		{
			try
			{
				var imageBytes = await File.ReadAllBytesAsync(imagePath);
				return await Task.Run(() => ProcesarImagen(imageBytes));
			}
			catch (Exception ex)
			{
				return new CedulaOCRResult
				{
					Exitoso = false,
					ErrorMessage = $"Error procesando imagen: {ex.Message}"
				};
			}
		}

		private static CedulaOCRResult ProcesarImagen(byte[] imageBytes)
		{
			try
			{
				using (var ms = new MemoryStream(imageBytes))
				using (var originalImage = System.Drawing.Image.FromStream(ms))
				using (var processedImage = PreprocesarImagen(originalImage))
				{
					return ExtraerTextoConTesseract(processedImage);
				}
			}
			catch (Exception ex)
			{
				return new CedulaOCRResult
				{
					Exitoso = false,
					ErrorMessage = $"Error en OCR: {ex.Message}"
				};
			}
		}

		private static Bitmap PreprocesarImagen(System.Drawing.Image originalImage)
		{
			var newWidth = originalImage.Width < 800 ? originalImage.Width * 2 : originalImage.Width;
			var newHeight = originalImage.Height < 600 ? originalImage.Height * 2 : originalImage.Height;

			var bitmap = new Bitmap(newWidth, newHeight);

			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
			}

			return MejorarContraste(bitmap);
		}

		private static Bitmap MejorarContraste(Bitmap bitmap)
		{
			var resultado = new Bitmap(bitmap.Width, bitmap.Height);

			for (int x = 0; x < bitmap.Width; x++)
			{
				for (int y = 0; y < bitmap.Height; y++)
				{
					var pixel = bitmap.GetPixel(x, y);
					var gris = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
					var nuevoGris = gris > 140 ? 255 : 0;
					resultado.SetPixel(x, y, Color.FromArgb(nuevoGris, nuevoGris, nuevoGris));
				}
			}

			return resultado;
		}

		private static CedulaOCRResult ExtraerTextoConTesseract(Bitmap image)
		{
			using (var engine = new TesseractEngine(_tessDataPath, "spa", EngineMode.Default))
			{
				// Configurar para números y letras
				engine.SetVariable("tessedit_char_whitelist", "0123456789-ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ");

				using (var pix = ConvertirBitmapAPix(image))
				using (var page = engine.Process(pix))
				{
					var textoCompleto = page.GetText();
					var confianza = page.GetMeanConfidence();

					var cedula = ExtraerCedulaDelTexto(textoCompleto);

					return new CedulaOCRResult
					{
						Exitoso = !string.IsNullOrEmpty(cedula),
						NumeroCedula = cedula,
						Confianza = confianza * 100,
						TextoCompleto = textoCompleto
					};
				}
			}
		}

		private static Pix ConvertirBitmapAPix(Bitmap bitmap)
		{
			using (var ms = new MemoryStream())
			{
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				return Pix.LoadFromMemory(ms.ToArray());
			}
		}

		private static string ExtraerCedulaDelTexto(string texto)
		{
			// Patrones para cédula dominicana
			var patrones = new[]
			{
				@"\b(\d{3}[-\s]?\d{7}[-\s]?\d)\b",  // 001-1276244-8
                @"\b(\d{11})\b",                     // 00112762448
                @"(\d{3})\s*[-]?\s*(\d{7})\s*[-]?\s*(\d)" // Flexible
            };

			foreach (var patron in patrones)
			{
				var matches = Regex.Matches(texto, patron);

				foreach (Match match in matches)
				{
					var numero = LimpiarYFormatear(match.Value);
					if (EsCedulaValida(numero))
					{
						return numero;
					}
				}
			}

			return null!;
		}

		private static string LimpiarYFormatear(string numero)
		{
			var soloNumeros = Regex.Replace(numero, @"[^\d]", "");

			if (soloNumeros.Length == 11)
			{
				return $"{soloNumeros.Substring(0, 3)}-{soloNumeros.Substring(3, 7)}-{soloNumeros.Substring(10, 1)}";
			}

			return numero;
		}

		private static bool EsCedulaValida(string cedula)
		{
			var soloNumeros = cedula.Replace("-", "");

			if (soloNumeros.Length != 11)
				return false;

			if (!Regex.IsMatch(soloNumeros, @"^\d{11}$"))
				return false;

			// No todos los dígitos iguales
			if (Regex.IsMatch(soloNumeros, @"^(\d)\1{10}$"))
				return false;

			return true;
		}
	}
}