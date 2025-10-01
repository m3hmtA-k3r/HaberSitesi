using Shared.Dtos;

namespace Business.Abstract
{
	public interface ISlaytService
	{
		List<SlaytlarDto> GetSlaytlar();
		SlaytlarDto GetSlaytById(int id);
		SlaytlarDto InsertSlayt(SlaytlarDto model);
		SlaytlarDto UpdateSlayt(SlaytlarDto model);
		bool DeleteSlayt(int id);
	}
}
