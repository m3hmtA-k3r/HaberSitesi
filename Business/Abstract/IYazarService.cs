using Application.DTOs;

namespace Business.Abstract
{
	public interface IYazarService
	{
		List<YazarlarDto> GetYazarlar();
		YazarlarDto GetYazarById(int id);
		YazarlarDto GetYazarByEmailPassword(string email, string password);
		YazarlarDto InsertYazar(YazarlarDto model);
		YazarlarDto UpdateYazar(YazarlarDto model);
		bool DeleteYazar(int id);
	}
}
