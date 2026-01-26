using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface IBlogApiRequest
	{
		List<BloglarDto> GetBloglar();
		List<BloglarDto> GetAktifBloglar();
		List<BloglarDto> GetBloglarByKategori(int kategoriId);
		List<BloglarDto> GetBloglarByYazar(int yazarId);
		BloglarDto GetBlogById(int blogId);
		BloglarDto BlogEkle(BloglarDto model);
		BloglarDto UpdateBlog(BloglarDto model);
		bool DeleteBlog(int blogId);
	}
}
