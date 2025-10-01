using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

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

		[HttpGet]
		[Route("DeleteYazar")]
		public bool DeleteYazar(int yazarId) => _yazarService.DeleteYazar(yazarId);

		[HttpPost]
		[Route("InsertYazar")]
		public YazarlarDto InsertYazar(YazarlarDto model) => _yazarService.InsertYazar(model);

		[HttpPost]
		[Route("UpdateYazar")]
		public YazarlarDto UpdateYazar(YazarlarDto model) => _yazarService.UpdateYazar(model);
	}
}
