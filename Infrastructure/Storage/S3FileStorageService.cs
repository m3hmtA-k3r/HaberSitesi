using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Storage
{
	/// <summary>
	/// AWS S3 file storage implementation
	/// Supports standard S3 and S3-compatible services (MinIO, DigitalOcean Spaces, etc.)
	/// </summary>
	public class S3FileStorageService : IFileStorageService, IDisposable
	{
		private readonly IAmazonS3 _s3Client;
		private readonly string _bucketName;
		private readonly string _baseUrl;
		private readonly string _keyPrefix;
		private bool _disposed;

		public S3FileStorageService(IConfiguration configuration)
		{
			var s3Config = configuration.GetSection("S3Storage");

			_bucketName = Environment.GetEnvironmentVariable("MASKER_S3_BUCKET")
				?? s3Config["BucketName"]
				?? throw new InvalidOperationException("S3 BucketName is not configured");

			var accessKey = Environment.GetEnvironmentVariable("MASKER_S3_ACCESS_KEY")
				?? s3Config["AccessKey"];

			var secretKey = Environment.GetEnvironmentVariable("MASKER_S3_SECRET_KEY")
				?? s3Config["SecretKey"];

			var regionName = Environment.GetEnvironmentVariable("MASKER_S3_REGION")
				?? s3Config["Region"]
				?? "eu-central-1";

			var serviceUrl = Environment.GetEnvironmentVariable("MASKER_S3_SERVICE_URL")
				?? s3Config["ServiceUrl"]; // For S3-compatible services

			_baseUrl = Environment.GetEnvironmentVariable("MASKER_S3_BASE_URL")
				?? s3Config["BaseUrl"]
				?? $"https://{_bucketName}.s3.{regionName}.amazonaws.com";

			_keyPrefix = s3Config["KeyPrefix"] ?? "uploads/";

			// Configure S3 client
			var config = new AmazonS3Config
			{
				RegionEndpoint = RegionEndpoint.GetBySystemName(regionName)
			};

			// Support S3-compatible services (MinIO, DigitalOcean Spaces, etc.)
			if (!string.IsNullOrEmpty(serviceUrl))
			{
				config.ServiceURL = serviceUrl;
				config.ForcePathStyle = true; // Required for MinIO and some S3-compatible services
			}

			if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
			{
				_s3Client = new AmazonS3Client(accessKey, secretKey, config);
			}
			else
			{
				// Use IAM role or default credentials chain
				_s3Client = new AmazonS3Client(config);
			}
		}

		public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
		{
			try
			{
				// Generate unique key
				var uniqueKey = $"{_keyPrefix}{Guid.NewGuid()}_{SanitizeFileName(fileName)}";

				using var transferUtility = new TransferUtility(_s3Client);
				var uploadRequest = new TransferUtilityUploadRequest
				{
					InputStream = fileStream,
					Key = uniqueKey,
					BucketName = _bucketName,
					ContentType = contentType,
					CannedACL = S3CannedACL.PublicRead // Make publicly readable
				};

				await transferUtility.UploadAsync(uploadRequest);

				// Return public URL
				return $"{_baseUrl.TrimEnd('/')}/{uniqueKey}";
			}
			catch (AmazonS3Exception ex)
			{
				throw new Exception($"S3 upload failed: {ex.Message}", ex);
			}
		}

		public async Task<bool> DeleteFileAsync(string fileUrl)
		{
			try
			{
				var key = ExtractKeyFromUrl(fileUrl);
				if (string.IsNullOrEmpty(key))
					return false;

				var deleteRequest = new DeleteObjectRequest
				{
					BucketName = _bucketName,
					Key = key
				};

				await _s3Client.DeleteObjectAsync(deleteRequest);
				return true;
			}
			catch (AmazonS3Exception)
			{
				return false;
			}
		}

		public async Task<Stream> GetFileAsync(string fileUrl)
		{
			try
			{
				var key = ExtractKeyFromUrl(fileUrl);
				if (string.IsNullOrEmpty(key))
					throw new FileNotFoundException($"Invalid file URL: {fileUrl}");

				var response = await _s3Client.GetObjectAsync(_bucketName, key);
				return response.ResponseStream;
			}
			catch (AmazonS3Exception ex)
			{
				throw new Exception($"S3 file retrieval failed: {ex.Message}", ex);
			}
		}

		public async Task<bool> FileExistsAsync(string fileUrl)
		{
			try
			{
				var key = ExtractKeyFromUrl(fileUrl);
				if (string.IsNullOrEmpty(key))
					return false;

				var request = new GetObjectMetadataRequest
				{
					BucketName = _bucketName,
					Key = key
				};

				await _s3Client.GetObjectMetadataAsync(request);
				return true;
			}
			catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				return false;
			}
		}

		private string ExtractKeyFromUrl(string fileUrl)
		{
			try
			{
				var uri = new Uri(fileUrl);
				var path = uri.AbsolutePath.TrimStart('/');

				// Handle bucket in path style URLs
				if (path.StartsWith(_bucketName + "/"))
				{
					path = path.Substring(_bucketName.Length + 1);
				}

				return path;
			}
			catch
			{
				return string.Empty;
			}
		}

		private static string SanitizeFileName(string fileName)
		{
			// Remove invalid characters for S3 keys
			var invalidChars = Path.GetInvalidFileNameChars();
			var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
			return sanitized.Replace(" ", "_");
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_s3Client?.Dispose();
				_disposed = true;
			}
			GC.SuppressFinalize(this);
		}
	}
}
