using Shared.Dtos;

namespace Business.Abstract
{
	public interface IYorumService
	{
		List<YorumlarDto> GetYorumlar();
		YorumlarDto GetYorumById(int id);
		YorumlarDto InsertYorum(YorumlarDto model);
		YorumlarDto UpdateYorum(YorumlarDto model);
		bool DeleteYorum(int id);
	}
}
