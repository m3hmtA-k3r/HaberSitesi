using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
	public class BlogYorumApiRequest : IBlogYorumApiRequest
	{
		private readonly IRequestService _requestService;
		public BlogYorumApiRequest(IRequestService requestService)
		{
			_requestService = requestService;
		}

		public List<BlogYorumlarDto> GetYorumlar() => _requestService.Get<List<BlogYorumlarDto>>("/BlogYorum/GetAllYorum");

		public List<BlogYorumlarDto> GetYorumlarByBlog(int blogId) => _requestService.Get<List<BlogYorumlarDto>>("/BlogYorum/GetYorumlarByBlog?blogId=" + blogId);

		public List<BlogYorumlarDto> GetOnayBekleyenYorumlar() => _requestService.Get<List<BlogYorumlarDto>>("/BlogYorum/GetOnayBekleyenYorumlar");

		public BlogYorumlarDto GetYorumById(int yorumId) => _requestService.Get<BlogYorumlarDto>("/BlogYorum/GetYorumById?yorumId=" + yorumId);

		public BlogYorumlarDto YorumEkle(BlogYorumlarDto model)
		{
			return _requestService.Post<BlogYorumlarDto>("/BlogYorum/InsertYorum", model);
		}

		public BlogYorumlarDto UpdateYorum(BlogYorumlarDto model)
		{
			return _requestService.Put<BlogYorumlarDto>("/BlogYorum/UpdateYorum", model);
		}

		public bool DeleteYorum(int yorumId)
		{
			return _requestService.Delete<bool>("/BlogYorum/DeleteYorum/" + yorumId);
		}

		public bool OnaylaYorum(int yorumId)
		{
			return _requestService.Post<bool>("/BlogYorum/OnaylaYorum?yorumId=" + yorumId, new { });
		}

		public bool ReddetYorum(int yorumId)
		{
			return _requestService.Post<bool>("/BlogYorum/ReddetYorum?yorumId=" + yorumId, new { });
		}
	}
}
