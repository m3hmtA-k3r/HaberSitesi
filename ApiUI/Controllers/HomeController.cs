using Microsoft.AspNetCore.Mvc;

namespace ApiUI.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		[HttpPost]
		[Route("Upload")]
		public string Upload(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return "";

			string newFileName = Guid.NewGuid() + "-" + file.FileName;
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/" + newFileName);

			using (var stream = new FileStream(path, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			return "/Uploads/" + newFileName;
		}
	}
}
