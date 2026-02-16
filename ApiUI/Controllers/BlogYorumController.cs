using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogYorumController : ControllerBase
	{
		private readonly IBlogYorumService _blogYorumService;
		public BlogYorumController(IBlogYorumService blogYorumService)
		{
			_blogYorumService = blogYorumService;
		}

		[HttpGet]
		[Route("GetAllYorum")]
		public List<BlogYorumlarDto> GetAllYorum() => _blogYorumService.GetYorumlar();

		[HttpGet]
		[Route("GetYorumById")]
		public BlogYorumlarDto GetYorumById(int yorumId) => _blogYorumService.GetYorumById(yorumId);

		[HttpGet]
		[Route("GetYorumlarByBlog")]
		public List<BlogYorumlarDto> GetYorumlarByBlog(int blogId) => _blogYorumService.GetYorumlarByBlog(blogId);

		[HttpGet]
		[Route("GetOnayliYorumlarByBlog")]
		public List<BlogYorumlarDto> GetOnayliYorumlarByBlog(int blogId) => _blogYorumService.GetOnayliYorumlarByBlog(blogId);

		[HttpGet]
		[Route("GetOnayBekleyenYorumlar")]
		public List<BlogYorumlarDto> GetOnayBekleyenYorumlar() => _blogYorumService.GetOnayBekleyenYorumlar();

		[HttpDelete]
		[Route("DeleteYorum/{yorumId}")]
		public IActionResult DeleteYorum(int yorumId)
		{
			var result = _blogYorumService.DeleteYorum(yorumId);
			if (!result)
				return NotFound(new { message = "Yorum bulunamadi" });
			return Ok(new { message = "Yorum silindi" });
		}

		[HttpPost]
		[Route("InsertYorum")]
		public BlogYorumlarDto InsertYorum(BlogYorumlarDto model) => _blogYorumService.InsertYorum(model);

		[HttpPut]
		[Route("UpdateYorum")]
		public BlogYorumlarDto UpdateYorum(BlogYorumlarDto model) => _blogYorumService.UpdateYorum(model);

		[HttpPost]
		[Route("OnaylaYorum")]
		public bool OnaylaYorum(int yorumId) => _blogYorumService.OnaylaYorum(yorumId);

		[HttpPost]
		[Route("ReddetYorum")]
		public bool ReddetYorum(int yorumId) => _blogYorumService.ReddetYorum(yorumId);
	}
}
