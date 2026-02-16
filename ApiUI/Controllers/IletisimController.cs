using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IletisimController : ControllerBase
	{
		private readonly IIletisimService _iletisimService;

		public IletisimController(IIletisimService iletisimService)
		{
			_iletisimService = iletisimService;
		}

		[HttpGet]
		[Route("GetAllMesaj")]
		public List<IletisimMesajlariDto> GetAllMesaj() => _iletisimService.GetMesajlar();

		[HttpGet]
		[Route("GetOkunmamisMesajlar")]
		public List<IletisimMesajlariDto> GetOkunmamisMesajlar() => _iletisimService.GetOkunmamisMesajlar();

		[HttpGet]
		[Route("GetMesajById")]
		public IletisimMesajlariDto GetMesajById(int mesajId) => _iletisimService.GetMesajById(mesajId);

		[HttpGet]
		[Route("GetOkunmamisMesajSayisi")]
		public IActionResult GetOkunmamisMesajSayisi()
		{
			var count = _iletisimService.GetOkunmamisMesajSayisi();
			return Ok(count);
		}

		[HttpPost]
		[Route("InsertMesaj")]
		public IletisimMesajlariDto InsertMesaj(IletisimMesajlariDto model) => _iletisimService.InsertMesaj(model);

		[HttpPut]
		[Route("OkunduOlarakIsaretle/{mesajId}")]
		public IActionResult OkunduOlarakIsaretle(int mesajId)
		{
			var result = _iletisimService.OkunduOlarakIsaretle(mesajId);
			if (!result)
				return NotFound(new { message = "Mesaj bulunamadi" });
			return Ok(new { message = "Mesaj okundu olarak isaretlendi" });
		}

		[HttpPut]
		[Route("CevaplandiOlarakIsaretle/{mesajId}")]
		public IActionResult CevaplandiOlarakIsaretle(int mesajId)
		{
			var result = _iletisimService.CevaplandiOlarakIsaretle(mesajId);
			if (!result)
				return NotFound(new { message = "Mesaj bulunamadi" });
			return Ok(new { message = "Mesaj cevaplandi olarak isaretlendi" });
		}

		[HttpDelete]
		[Route("DeleteMesaj/{mesajId}")]
		public IActionResult DeleteMesaj(int mesajId)
		{
			var result = _iletisimService.DeleteMesaj(mesajId);
			if (!result)
				return NotFound(new { message = "Mesaj bulunamadi" });
			return Ok(new { message = "Mesaj silindi" });
		}
	}
}
