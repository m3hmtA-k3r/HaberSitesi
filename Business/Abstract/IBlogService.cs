using Application.DTOs;

namespace Business.Abstract
{
	public interface IBlogService
	{
		List<BloglarDto> GetBloglar();
		List<BloglarDto> GetAktifBloglar();
		List<BloglarDto> GetBloglarByKategori(int kategoriId);
		List<BloglarDto> GetBloglarByYazar(int yazarId);
		BloglarDto GetBlogById(int id);
		BloglarDto InsertBlog(BloglarDto model);
		BloglarDto UpdateBlog(BloglarDto model);
		bool DeleteBlog(int id);
		void IncrementGoruntulenmeSayisi(int id);
	}
}
