using System.Security.Cryptography;
using System.Text;

namespace SADVO.Core.Application.Helpers
{
	public static class PasswordEncryption
	{
		public static string ComputeSha256Hash(string password)
		{
			using (SHA256 sha256 = SHA256.Create()) 
			{
				byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				//
				StringBuilder sb = new StringBuilder();
				foreach (var items in bytes)
				{
					sb.Append(items.ToString("x2"));

				}

				return sb.ToString();
			}
		}
	}
}
