using SADVO.Core.Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace SADVO.Infrastructure.Shared.Services
{
	public class EmailService : IEmailServices
	{
		private static readonly string _smtpServer = "smtp.gmail.com";
		private static readonly int _port = 587;
		private static readonly string _username = "sadvovotingsystem@gmail.com";
		private static readonly string _password = "nwpp hcnk epgy mwwh";


		public EmailService()
		{

		}
		public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
		{
			try
			{
				using (var client = new SmtpClient(_smtpServer, _port))
				{
					client.EnableSsl = true;
					client.UseDefaultCredentials = false;
					client.Credentials = new NetworkCredential(_username, _password);

					var message = new MailMessage(_username, toEmail, subject, body);

					await client.SendMailAsync(message);
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error enviando email: {ex.Message}");
				return false;
			}
		}
	}
}

// Ejemplo de uso:
// bool resultado = await EmailService.SendEmailAsync("usuario@ejemplo.com", "Asunto", "Mensaje");