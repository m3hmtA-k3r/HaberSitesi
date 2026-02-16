using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface IIletisimApiRequest
	{
		List<IletisimMesajlariDto> GetAllMesaj();
		List<IletisimMesajlariDto> GetOkunmamisMesajlar();
		IletisimMesajlariDto GetMesajById(int mesajId);
		int GetOkunmamisMesajSayisi();
		IletisimMesajlariDto InsertMesaj(IletisimMesajlariDto model);
		bool OkunduOlarakIsaretle(int mesajId);
		bool CevaplandiOlarakIsaretle(int mesajId);
		bool DeleteMesaj(int mesajId);
	}
}
