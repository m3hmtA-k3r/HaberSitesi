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
		[Route("GetHaberById")]
		public HaberlerDto GetHaberById(int haberId) => _haberService.GetHaberById(haberId);

		[HttpGet]
		[Route("DeleteHaber")]
		public bool DeleteHaber(int haberId) => _haberService.DeleteHaber(haberId);

		[HttpPost]
		[Route("InsertHaber")]
		public HaberlerDto InsertHaber(HaberlerDto model) => _haberService.InsertHaber(model);

		[HttpPost]
		[Route("UpdateHaber")]
		public HaberlerDto UpdateHaber(HaberlerDto model) => _haberService.UpdateHaber(model);
	}
}
