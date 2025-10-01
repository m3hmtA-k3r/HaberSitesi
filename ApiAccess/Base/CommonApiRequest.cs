using ApiAccess.Abstract;
using Microsoft.AspNetCore.Http;

namespace ApiAccess.Base
{
	public class CommonApiRequest : ICommonApiRequest
	{
		public string Upload(IFormFile file)
		{
			var stream = new MemoryStream();
			file.CopyTo(stream);
			var bytes = stream.ToArray();
			var fileContent = new ByteArrayContent(bytes);

			MultipartFormDataContent content = new MultipartFormDataContent();
			content.Add(fileContent, "file", file.FileName);

			using var client = new HttpClient();

			var response = client.PostAsync("https://localhost:7063/api/home/upload", content).Result;

			if (response.IsSuccessStatusCode)
				return response.Content.ReadAsStringAsync().Result;
			
			else
				return null;
		}
	}
}
