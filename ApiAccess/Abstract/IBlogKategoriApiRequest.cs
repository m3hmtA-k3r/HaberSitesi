using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface IBlogKategoriApiRequest
	{
		List<BlogKategorilerDto> GetKategoriler();
		List<BlogKategorilerDto> GetAktifKategoriler();
		BlogKategorilerDto GetKategoriById(int kategoriId);
		BlogKategorilerDto KategoriEkle(BlogKategorilerDto model);
		BlogKategorilerDto UpdateKategori(BlogKategorilerDto model);
		bool DeleteKategori(int kategoriId);
	}
}
