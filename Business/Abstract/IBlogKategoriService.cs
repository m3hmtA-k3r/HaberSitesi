using Application.DTOs;

namespace Business.Abstract
{
	public interface IBlogKategoriService
	{
		List<BlogKategorilerDto> GetKategoriler();
		List<BlogKategorilerDto> GetAktifKategoriler();
		BlogKategorilerDto GetKategoriById(int id);
		BlogKategorilerDto InsertKategori(BlogKategorilerDto model);
		BlogKategorilerDto UpdateKategori(BlogKategorilerDto model);
		bool DeleteKategori(int id);
	}
}
