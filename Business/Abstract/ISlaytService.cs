using Application.DTOs;

namespace Business.Abstract
{
	public interface ISlaytService
	{
		int GetUnpublishedSlidesCount();



        List<SlaytlarDto> GetSlaytlar();
		SlaytlarDto GetSlaytById(int id);
		SlaytlarDto InsertSlayt(SlaytlarDto model);
		SlaytlarDto UpdateSlayt(SlaytlarDto model);
		bool DeleteSlayt(int id);
	}
}
