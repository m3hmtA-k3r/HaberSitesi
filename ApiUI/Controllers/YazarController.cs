using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class YazarController : ControllerBase
	{
		private readonly IYazarService _yazarService;
		public YazarController(IYazarService yazarService)
		{
			_yazarService = yazarService;
		}

		[HttpGet]
		[Route("GetAllYazar")]
		public List<YazarlarDto> GetAllYazar() => _yazarService.GetYazarlar();

		[HttpGet]
		[Route("GetYazarById")]
		public YazarlarDto GetYazarById(int yazarId) => _yazarService.GetYazarById(yazarId);

		[HttpGet]
		[Route("GetYazarByEmailPassword")]
		public YazarlarDto GetYazarByEmailPassword(string email, string password) => _yazarService.GetYazarByEmailPassword(email,password);

		[HttpDelete]
		[Route("DeleteYazar/{yazarId}")]
		public IActionResult DeleteYazar(int yazarId)
		{
			var result = _yazarService.DeleteYazar(yazarId);
			if (!result)
				return NotFound(new { message = "Yazar bulunamadi" });
			return Ok(new { message = "Yazar silindi" });
		}

		[HttpPost]
		[Route("InsertYazar")]
		public YazarlarDto InsertYazar(YazarlarDto model) => _yazarService.InsertYazar(model);

		[HttpPut]
		[Route("UpdateYazar")]
		public YazarlarDto UpdateYazar(YazarlarDto model) => _yazarService.UpdateYazar(model);
	}
}
