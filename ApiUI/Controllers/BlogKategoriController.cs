using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogKategoriController : ControllerBase
	{
		private readonly IBlogKategoriService _blogKategoriService;
		public BlogKategoriController(IBlogKategoriService blogKategoriService)
		{
			_blogKategoriService = blogKategoriService;
		}

		[HttpGet]
		[Route("GetAllKategori")]
		public List<BlogKategorilerDto> GetAllKategori() => _blogKategoriService.GetKategoriler();

		[HttpGet]
		[Route("GetAktifKategoriler")]
		public List<BlogKategorilerDto> GetAktifKategoriler() => _blogKategoriService.GetAktifKategoriler();

		[HttpGet]
		[Route("GetKategoriById")]
		public BlogKategorilerDto GetKategoriById(int kategoriId) => _blogKategoriService.GetKategoriById(kategoriId);

		[HttpDelete]
		[Route("DeleteKategori/{kategoriId}")]
		public IActionResult DeleteKategori(int kategoriId)
		{
			var result = _blogKategoriService.DeleteKategori(kategoriId);
			if (!result)
				return NotFound(new { message = "Kategori bulunamadi" });
			return Ok(new { message = "Kategori silindi" });
		}

		[HttpPost]
		[Route("InsertKategori")]
		public BlogKategorilerDto InsertKategori(BlogKategorilerDto model) => _blogKategoriService.InsertKategori(model);

		[HttpPut]
		[Route("UpdateKategori")]
		public BlogKategorilerDto UpdateKategori(BlogKategorilerDto model) => _blogKategoriService.UpdateKategori(model);
	}
}
