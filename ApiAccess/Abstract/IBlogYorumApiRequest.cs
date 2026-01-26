using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface IBlogYorumApiRequest
	{
		List<BlogYorumlarDto> GetYorumlar();
		List<BlogYorumlarDto> GetYorumlarByBlog(int blogId);
		List<BlogYorumlarDto> GetOnayBekleyenYorumlar();
		BlogYorumlarDto GetYorumById(int yorumId);
		BlogYorumlarDto YorumEkle(BlogYorumlarDto model);
		BlogYorumlarDto UpdateYorum(BlogYorumlarDto model);
		bool DeleteYorum(int yorumId);
		bool OnaylaYorum(int yorumId);
		bool ReddetYorum(int yorumId);
	}
}
