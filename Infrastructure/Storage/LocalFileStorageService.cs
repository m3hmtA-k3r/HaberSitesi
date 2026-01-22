using Microsoft.Extensions.Configuration;

namespace Infrastructure.Storage
{
	/// <summary>
	/// Local file system storage implementation
	/// Stores files in wwwroot/Uploads directory
	/// </summary>
	public class LocalFileStorageService : IFileStorageService
	{
		private readonly string _uploadPath;
		private readonly string _baseUrl;

		public LocalFileStorageService(IConfiguration configuration)
		{
			_uploadPath = configuration["FileStorage:LocalPath"] ?? "wwwroot/Uploads";
			_baseUrl = configuration["FileStorage:BaseUrl"] ?? "http://localhost:5100";

			// Ensure upload directory exists
			if (!Directory.Exists(_uploadPath))
			{
				Directory.CreateDirectory(_uploadPath);
			}
		}

		public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
		{
			try
			{
				// Generate unique file name to avoid conflicts
				var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
				var filePath = Path.Combine(_uploadPath, uniqueFileName);

				// Save file
				using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
				{
					await fileStream.CopyToAsync(fileStreamOutput);
				}

				// Return public URL
				return $"{_baseUrl}/Uploads/{uniqueFileName}";
			}
			catch (Exception ex)
			{
				throw new Exception($"File upload failed: {ex.Message}", ex);
			}
		}

		public Task<bool> DeleteFileAsync(string fileUrl)
		{
			try
			{
				// Extract file name from URL
				var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
				var filePath = Path.Combine(_uploadPath, fileName);

				if (File.Exists(filePath))
				{
					File.Delete(filePath);
					return Task.FromResult(true);
				}

				return Task.FromResult(false);
			}
			catch
			{
				return Task.FromResult(false);
			}
		}

		public Task<Stream> GetFileAsync(string fileUrl)
		{
			try
			{
				var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
				var filePath = Path.Combine(_uploadPath, fileName);

				if (File.Exists(filePath))
				{
					return Task.FromResult<Stream>(new FileStream(filePath, FileMode.Open, FileAccess.Read));
				}

				throw new FileNotFoundException($"File not found: {fileUrl}");
			}
			catch (Exception ex)
			{
				throw new Exception($"File retrieval failed: {ex.Message}", ex);
			}
		}

		public Task<bool> FileExistsAsync(string fileUrl)
		{
			try
			{
				var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
				var filePath = Path.Combine(_uploadPath, fileName);
				return Task.FromResult(File.Exists(filePath));
			}
			catch
			{
				return Task.FromResult(false);
			}
		}
	}
}
