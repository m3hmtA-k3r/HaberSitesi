using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class YorumController : ControllerBase
	{
		private readonly IYorumService _yorumService;
		public YorumController(IYorumService yorumService)
		{
			_yorumService = yorumService;
		}

		[HttpGet]
		[Route("GetPendingCommentsCount")]
        public IActionResult GetPendingCommentsCount()
		{
            var sayi = _yorumService.GetOnayBekleyenYorumSayisi();
            return Ok(sayi);
        }



        [HttpGet]
		[Route("GetAllYorum")]
		public List<YorumlarDto> GetAllYorum() => _yorumService.GetYorumlar();

		[HttpGet]
		[Route("GetYorumById")]
		public YorumlarDto GetYorumById(int yorumId) => _yorumService.GetYorumById(yorumId);

		[HttpDelete]
		[Route("DeleteYorum/{yorumId}")]
		public IActionResult DeleteYorum(int yorumId)
		{
			var result = _yorumService.DeleteYorum(yorumId);
			if (!result)
				return NotFound(new { message = "Yorum bulunamadi" });
			return Ok(new { message = "Yorum silindi" });
		}

		[HttpPost]
		[Route("InsertYorum")]
		public YorumlarDto InsertYorum(YorumlarDto model) => _yorumService.InsertYorum(model);

		[HttpPut]
		[Route("UpdateYorum")]
		public YorumlarDto UpdateYorum(YorumlarDto model) => _yorumService.UpdateYorum(model);
	}
}
