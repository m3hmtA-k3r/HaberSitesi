using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface ISlaytApiRequest
	{
		List<SlaytlarDto> GetAllSlayt();
		SlaytlarDto InsertSlayt(SlaytlarDto model);
		SlaytlarDto GetSlaytById(int slaytId);
		SlaytlarDto UpdateSlayt(SlaytlarDto model);
		bool DeleteSlayt(int slaytId);
	}
}
