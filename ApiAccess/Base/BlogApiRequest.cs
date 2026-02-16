using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
	public class BlogApiRequest : IBlogApiRequest
	{
		private readonly IRequestService _requestService;
		public BlogApiRequest(IRequestService requestService)
		{
			_requestService = requestService;
		}

		public List<BloglarDto> GetBloglar() => _requestService.Get<List<BloglarDto>>("/Blog/GetAllBlog");

		public List<BloglarDto> GetAktifBloglar() => _requestService.Get<List<BloglarDto>>("/Blog/GetAktifBloglar");

		public List<BloglarDto> GetBloglarByKategori(int kategoriId) => _requestService.Get<List<BloglarDto>>("/Blog/GetBloglarByKategori?kategoriId=" + kategoriId);

		public List<BloglarDto> GetBloglarByYazar(int yazarId) => _requestService.Get<List<BloglarDto>>("/Blog/GetBloglarByYazar?yazarId=" + yazarId);

		public BloglarDto GetBlogById(int blogId) => _requestService.Get<BloglarDto>("/Blog/GetBlogById?blogId=" + blogId);

		public BloglarDto BlogEkle(BloglarDto model)
		{
			return _requestService.Post<BloglarDto>("/Blog/InsertBlog", model);
		}

		public BloglarDto UpdateBlog(BloglarDto model)
		{
			return _requestService.Put<BloglarDto>("/Blog/UpdateBlog", model);
		}

		public bool DeleteBlog(int blogId)
		{
			return _requestService.Delete<bool>("/Blog/DeleteBlog/" + blogId);
		}
	}
}
