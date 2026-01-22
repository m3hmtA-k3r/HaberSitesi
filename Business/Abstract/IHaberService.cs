using Application.DTOs;

namespace Business.Abstract
{
	public interface IHaberService
	{
		List<HaberlerDto> GetHaberler();
		HaberlerDto GetHaberById(int id);
		HaberlerDto InsertHaber(HaberlerDto model);
		HaberlerDto UpdateHaber(HaberlerDto model);
		bool DeleteHaber(int id);

	}
}
