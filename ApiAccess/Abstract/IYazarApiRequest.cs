using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface IYazarApiRequest
	{
		YazarlarDto GetYazarByEmailPassword (string email, string password);
		List<YazarlarDto> GetAllYazar();
		YazarlarDto InsertYazar(YazarlarDto model);
		YazarlarDto GetYazarById(int yazarId);
		YazarlarDto UpdateYazar(YazarlarDto model);
		bool DeleteYazar(int yazarId);
	}
}
