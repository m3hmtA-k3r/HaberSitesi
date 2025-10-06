using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface ISlaytApiRequest
	{
		int GetUnpublishedSlidesCount();


        List<SlaytlarDto> GetAllSlayt();
		SlaytlarDto InsertSlayt(SlaytlarDto model);
		SlaytlarDto GetSlaytById(int slaytId);
		SlaytlarDto UpdateSlayt(SlaytlarDto model);
		bool DeleteSlayt(int slaytId);
	}
}
