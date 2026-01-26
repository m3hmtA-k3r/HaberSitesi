using Application.DTOs;

namespace Business.Abstract
{
	public interface IBlogYorumService
	{
		List<BlogYorumlarDto> GetYorumlar();
		List<BlogYorumlarDto> GetYorumlarByBlog(int blogId);
		List<BlogYorumlarDto> GetOnayliYorumlarByBlog(int blogId);
		List<BlogYorumlarDto> GetOnayBekleyenYorumlar();
		BlogYorumlarDto GetYorumById(int id);
		BlogYorumlarDto InsertYorum(BlogYorumlarDto model);
		BlogYorumlarDto UpdateYorum(BlogYorumlarDto model);
		bool DeleteYorum(int id);
		bool OnaylaYorum(int id);
		bool ReddetYorum(int id);
	}
}
