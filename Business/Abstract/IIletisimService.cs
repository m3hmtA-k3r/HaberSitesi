using Application.DTOs;

namespace Business.Abstract
{
	public interface IIletisimService
	{
		List<IletisimMesajlariDto> GetMesajlar();
		List<IletisimMesajlariDto> GetOkunmamisMesajlar();
		IletisimMesajlariDto GetMesajById(int id);
		IletisimMesajlariDto InsertMesaj(IletisimMesajlariDto model);
		bool OkunduOlarakIsaretle(int id);
		bool CevaplandiOlarakIsaretle(int id);
		bool DeleteMesaj(int id);
		int GetOkunmamisMesajSayisi();
	}
}
