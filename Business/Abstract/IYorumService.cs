using Application.DTOs;

namespace Business.Abstract
{
	public interface IYorumService
	{
        int GetOnayBekleyenYorumSayisi();

        List<YorumlarDto> GetYorumlar();
		YorumlarDto GetYorumById(int id);
		YorumlarDto InsertYorum(YorumlarDto model);
		YorumlarDto UpdateYorum(YorumlarDto model);
		bool DeleteYorum(int id);
	}
}
