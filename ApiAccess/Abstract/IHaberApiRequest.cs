using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IHaberApiRequest
    {
        List<HaberlerDto> GetAllHaber();
		HaberlerDto HaberEkle(HaberlerDto model);
		HaberlerDto GetHaberById(int haberId);
		HaberlerDto UpdateHaber(HaberlerDto model);
		bool DeleteHaber(int haberId);
	}
}
