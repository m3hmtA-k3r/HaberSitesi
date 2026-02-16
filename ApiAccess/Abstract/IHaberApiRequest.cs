using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IHaberApiRequest
    {
        List<HaberlerDto> GetAllHaber();
		PagedResultDto<HaberlerDto> GetHaberlerPaged(int sayfa = 1, int boyut = 9, bool? aktif = null, int? kategoriId = null, string siralama = "yeni");
		HaberlerDto HaberEkle(HaberlerDto model);
		HaberlerDto GetHaberById(int haberId);
		HaberlerDto UpdateHaber(HaberlerDto model);
		bool DeleteHaber(int haberId);
	}
}
