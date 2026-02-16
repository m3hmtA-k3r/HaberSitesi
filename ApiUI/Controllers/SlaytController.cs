using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SlaytController : ControllerBase
	{
		private readonly ISlaytService _slaytService;
		public SlaytController(ISlaytService slaytService)
		{
			_slaytService = slaytService;
		}

        [HttpGet]
        [Route("GetUnpublishedSlidesCount")]
		public IActionResult GetUnpublishedSlidesCount()
        {
            var count = _slaytService.GetUnpublishedSlidesCount();
            return Ok(count);
        }

        [HttpGet]
		[Route("GetAllSlayt")]
		public List<SlaytlarDto> GetAllSlayt() => _slaytService.GetSlaytlar();

		[HttpGet]
		[Route("GetSlaytById")]
		public SlaytlarDto GetSlaytById(int slaytId) => _slaytService.GetSlaytById(slaytId);

		[HttpDelete]
		[Route("DeleteSlayt/{slaytId}")]
		public IActionResult DeleteSlayt(int slaytId)
		{
			var result = _slaytService.DeleteSlayt(slaytId);
			if (!result)
				return NotFound(new { message = "Slayt bulunamadi" });
			return Ok(new { message = "Slayt silindi" });
		}

		[HttpPost]
		[Route("InsertSlayt")]
		public SlaytlarDto InsertSlayt(SlaytlarDto model) => _slaytService.InsertSlayt(model);

		[HttpPut]
		[Route("UpdateSlayt")]
		public SlaytlarDto UpdateSlayt(SlaytlarDto model) => _slaytService.UpdateSlayt(model);
	}
}
