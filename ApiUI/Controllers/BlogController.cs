using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly IBlogService _blogService;
		public BlogController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		[HttpGet]
		[Route("GetAllBlog")]
		public List<BloglarDto> GetAllBlog() => _blogService.GetBloglar();

		[HttpGet]
		[Route("GetAktifBloglar")]
		public List<BloglarDto> GetAktifBloglar() => _blogService.GetAktifBloglar();

		[HttpGet]
		[Route("GetBlogById")]
		public BloglarDto GetBlogById(int blogId) => _blogService.GetBlogById(blogId);

		[HttpGet]
		[Route("GetBloglarByKategori")]
		public List<BloglarDto> GetBloglarByKategori(int kategoriId) => _blogService.GetBloglarByKategori(kategoriId);

		[HttpGet]
		[Route("GetBloglarByYazar")]
		public List<BloglarDto> GetBloglarByYazar(int yazarId) => _blogService.GetBloglarByYazar(yazarId);

		[HttpGet]
		[Route("DeleteBlog")]
		public bool DeleteBlog(int blogId) => _blogService.DeleteBlog(blogId);

		[HttpPost]
		[Route("InsertBlog")]
		public BloglarDto InsertBlog(BloglarDto model) => _blogService.InsertBlog(model);

		[HttpPost]
		[Route("UpdateBlog")]
		public BloglarDto UpdateBlog(BloglarDto model) => _blogService.UpdateBlog(model);

		[HttpPost]
		[Route("IncrementGoruntulenmeSayisi")]
		public IActionResult IncrementGoruntulenmeSayisi(int blogId)
		{
			_blogService.IncrementGoruntulenmeSayisi(blogId);
			return Ok();
		}
	}
}
