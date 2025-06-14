using Microsoft.AspNetCore.Http;

namespace SADVO.Core.Application.Interfaces
{
	public interface IImageUploader
	{
		Task<string> UploadImageAsync(IFormFile file);
	}
}
