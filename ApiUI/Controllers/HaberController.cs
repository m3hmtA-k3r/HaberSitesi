using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HaberController : ControllerBase
	{
        private readonly IHaberService _haberService;
        public HaberController(IHaberService haberService)
        {
			_haberService = haberService;
		}

		[HttpGet]
		[Route("GetAllHaber")]
		public List<HaberlerDto> GetAllHaber() => _haberService.GetHaberler();

		[HttpGet]
		[Route("GetHaberlerPaged")]
		public PagedResultDto<HaberlerDto> GetHaberlerPaged(int sayfa = 1, int boyut = 9, bool? aktif = null, int? kategoriId = null, string siralama = "yeni")
			=> _haberService.GetHaberlerPaged(sayfa, boyut, aktif, kategoriId, siralama);

		[HttpGet]
		[Route("GetHaberById")]
		public HaberlerDto GetHaberById(int haberId) => _haberService.GetHaberById(haberId);

		[HttpDelete]
		[Route("DeleteHaber/{haberId}")]
		public IActionResult DeleteHaber(int haberId)
		{
			var result = _haberService.DeleteHaber(haberId);
			if (!result)
				return NotFound(new { message = "Haber bulunamadi" });
			return Ok(new { message = "Haber silindi" });
		}

		[HttpPost]
		[Route("InsertHaber")]
		public HaberlerDto InsertHaber(HaberlerDto model) => _haberService.InsertHaber(model);

		[HttpPut]
		[Route("UpdateHaber")]
		public HaberlerDto UpdateHaber(HaberlerDto model) => _haberService.UpdateHaber(model);
	}
}
