namespace SADVO.Core.Application.Interfaces
{
	public interface IEmailServices
	{
		Task<bool> SendEmailAsync(string toEmail, string subject, string body);
	}
}
