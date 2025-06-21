using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace SADVO.Core.Application.Helpers
{
	public static class UploadFile
	{
		public static string Upload(IFormFile file, int id, string folderName, bool isEditMode = false, string imagePath = "")
		{
			if (isEditMode && file == null)
			{
				return imagePath;
			}

			string basePath = $"Images/{folderName}/{id}";
			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", basePath);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			Guid guid = Guid.NewGuid();
			FileInfo fileInfo = new FileInfo(file.FileName);
			string fileName = guid + fileInfo.Extension;

			string fullFilePath = Path.Combine(path, fileName);

			using (var stream = new FileStream(fullFilePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			return $"{basePath}/{fileName}";
		}

		// New overload for when you don't have an ID yet
		public static string UploadWithGuid(IFormFile file, string folderName, bool isEditMode = false, string imagePath = "")
		{
			if (isEditMode && file == null)
			{
				return imagePath;
			}

			Guid folderGuid = Guid.NewGuid();
			string basePath = $"Images/{folderName}/{folderGuid}";
			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", basePath);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			Guid fileGuid = Guid.NewGuid();
			FileInfo fileInfo = new FileInfo(file.FileName);
			string fileName = fileGuid + fileInfo.Extension;

			string fullFilePath = Path.Combine(path, fileName);

			using (var stream = new FileStream(fullFilePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			return $"{basePath}/{fileName}";
		}
	}
}