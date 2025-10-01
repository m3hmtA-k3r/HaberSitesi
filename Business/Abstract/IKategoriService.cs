using Shared.Dtos;

namespace Business.Abstract
{
	public interface IKategoriService
	{
		List<KategorilerDto> GetKategoriler();
		KategorilerDto GetKategoriById(int id);
		KategorilerDto InsertKategori(KategorilerDto model);
		KategorilerDto UpdateKategori(KategorilerDto model);
		bool DeleteKategori(int id);
	}
}
