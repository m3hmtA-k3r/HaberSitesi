using Application.DTOs;

namespace Business.Abstract
{
	public interface IHaberService
	{
		List<HaberlerDto> GetHaberler();
		PagedResultDto<HaberlerDto> GetHaberlerPaged(int page = 1, int pageSize = 9, bool? aktif = null, int? kategoriId = null, string siralama = "yeni");
		HaberlerDto GetHaberById(int id);
		HaberlerDto InsertHaber(HaberlerDto model);
		HaberlerDto UpdateHaber(HaberlerDto model);
		bool DeleteHaber(int id);
	}
}
