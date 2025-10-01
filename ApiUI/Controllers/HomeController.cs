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
			string newFileName = Guid.NewGuid() + "-" + file.FileName;
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/" + newFileName);
			var stream = new FileStream(path,FileMode.Create);
			file.CopyTo(stream);

			return "/Uploads/" + newFileName;
		}
	}
}
