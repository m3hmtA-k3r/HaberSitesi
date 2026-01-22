namespace Infrastructure.Storage
{
	/// <summary>
	/// Interface for file storage operations
	/// Supports multiple storage providers (Local, AWS S3, Azure Blob, etc.)
	/// </summary>
	public interface IFileStorageService
	{
		/// <summary>
		/// Upload a file and return the file URL
		/// </summary>
		Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);

		/// <summary>
		/// Delete a file by URL or path
		/// </summary>
		Task<bool> DeleteFileAsync(string fileUrl);

		/// <summary>
		/// Get file as stream
		/// </summary>
		Task<Stream> GetFileAsync(string fileUrl);

		/// <summary>
		/// Check if file exists
		/// </summary>
		Task<bool> FileExistsAsync(string fileUrl);
	}
}
