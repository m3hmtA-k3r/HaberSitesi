using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IKategoriApiRequest
    {
        List<KategorilerDto> GetKategoriler();
        KategorilerDto KategoriEkle(KategorilerDto model);
		KategorilerDto GetKategoriById(int kategoriId);
        KategorilerDto UpdateKategori(KategorilerDto model);
        bool DeleteKategori(int kategoriId);
	}
}
