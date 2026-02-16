using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Storage
{
	/// <summary>
	/// Hybrid file storage service with S3 primary and local fallback
	/// Automatically falls back to local storage if S3 is unavailable
	/// </summary>
	public class HybridFileStorageService : IFileStorageService, IDisposable
	{
		private readonly S3FileStorageService? _s3Service;
		private readonly LocalFileStorageService _localService;
		private readonly ILogger<HybridFileStorageService>? _logger;
		private readonly bool _s3Available;
		private bool _disposed;

		public HybridFileStorageService(IConfiguration configuration, ILogger<HybridFileStorageService>? logger = null)
		{
			_logger = logger;
			_localService = new LocalFileStorageService(configuration);

			// Try to initialize S3 service
			try
			{
				var s3Bucket = Environment.GetEnvironmentVariable("MASKER_S3_BUCKET")
					?? configuration["S3Storage:BucketName"];

				if (!string.IsNullOrEmpty(s3Bucket))
				{
					_s3Service = new S3FileStorageService(configuration);
					_s3Available = true;
					_logger?.LogInformation("S3 storage initialized successfully. Using S3 as primary storage.");
				}
				else
				{
					_s3Available = false;
					_logger?.LogInformation("S3 not configured. Using local storage only.");
				}
			}
			catch (Exception ex)
			{
				_s3Available = false;
				_logger?.LogWarning(ex, "Failed to initialize S3 storage. Using local storage as fallback.");
			}
		}

		public bool IsS3Available => _s3Available;

		public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
		{
			if (_s3Available && _s3Service != null)
			{
				try
				{
					// Reset stream position for potential retry
					var originalPosition = fileStream.CanSeek ? fileStream.Position : 0;

					var result = await _s3Service.UploadFileAsync(fileStream, fileName, contentType);
					_logger?.LogDebug("File uploaded to S3: {FileName}", fileName);
					return result;
				}
				catch (Exception ex)
				{
					_logger?.LogWarning(ex, "S3 upload failed, falling back to local storage: {FileName}", fileName);

					// Reset stream position for fallback
					if (fileStream.CanSeek)
					{
						fileStream.Position = 0;
					}
				}
			}

			// Fallback to local storage
			var localResult = await _localService.UploadFileAsync(fileStream, fileName, contentType);
			_logger?.LogDebug("File uploaded to local storage: {FileName}", fileName);
			return localResult;
		}

		public async Task<bool> DeleteFileAsync(string fileUrl)
		{
			// Try to detect storage type from URL
			if (IsS3Url(fileUrl) && _s3Service != null)
			{
				try
				{
					return await _s3Service.DeleteFileAsync(fileUrl);
				}
				catch (Exception ex)
				{
					_logger?.LogWarning(ex, "S3 delete failed: {FileUrl}", fileUrl);
					return false;
				}
			}

			return await _localService.DeleteFileAsync(fileUrl);
		}

		public async Task<Stream> GetFileAsync(string fileUrl)
		{
			if (IsS3Url(fileUrl) && _s3Service != null)
			{
				try
				{
					return await _s3Service.GetFileAsync(fileUrl);
				}
				catch (Exception ex)
				{
					_logger?.LogWarning(ex, "S3 get failed, trying local: {FileUrl}", fileUrl);
				}
			}

			return await _localService.GetFileAsync(fileUrl);
		}

		public async Task<bool> FileExistsAsync(string fileUrl)
		{
			if (IsS3Url(fileUrl) && _s3Service != null)
			{
				try
				{
					return await _s3Service.FileExistsAsync(fileUrl);
				}
				catch
				{
					// Fall through to local check
				}
			}

			return await _localService.FileExistsAsync(fileUrl);
		}

		private bool IsS3Url(string fileUrl)
		{
			if (string.IsNullOrEmpty(fileUrl))
				return false;

			// Check for common S3 URL patterns
			return fileUrl.Contains(".s3.") ||
				   fileUrl.Contains("s3.amazonaws.com") ||
				   fileUrl.Contains(".digitaloceanspaces.com") ||
				   fileUrl.Contains("minio");
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_s3Service?.Dispose();
				_disposed = true;
			}
			GC.SuppressFinalize(this);
		}
	}
}
