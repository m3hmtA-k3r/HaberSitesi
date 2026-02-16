using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
	public class IletisimApiRequest : IIletisimApiRequest
	{
		private readonly IRequestService _requestService;

		public IletisimApiRequest(IRequestService requestService)
		{
			_requestService = requestService;
		}

		public List<IletisimMesajlariDto> GetAllMesaj()
		{
			return _requestService.Get<List<IletisimMesajlariDto>>("Iletisim/GetAllMesaj");
		}

		public List<IletisimMesajlariDto> GetOkunmamisMesajlar()
		{
			return _requestService.Get<List<IletisimMesajlariDto>>("Iletisim/GetOkunmamisMesajlar");
		}

		public IletisimMesajlariDto GetMesajById(int mesajId)
		{
			return _requestService.Get<IletisimMesajlariDto>("Iletisim/GetMesajById?mesajId=" + mesajId);
		}

		public int GetOkunmamisMesajSayisi()
		{
			return _requestService.Get<int>("Iletisim/GetOkunmamisMesajSayisi");
		}

		public IletisimMesajlariDto InsertMesaj(IletisimMesajlariDto model)
		{
			return _requestService.Post<IletisimMesajlariDto>("Iletisim/InsertMesaj", model);
		}

		public bool OkunduOlarakIsaretle(int mesajId)
		{
			return _requestService.Put<bool>("Iletisim/OkunduOlarakIsaretle/" + mesajId, new { });
		}

		public bool CevaplandiOlarakIsaretle(int mesajId)
		{
			return _requestService.Put<bool>("Iletisim/CevaplandiOlarakIsaretle/" + mesajId, new { });
		}

		public bool DeleteMesaj(int mesajId)
		{
			return _requestService.Delete<bool>("Iletisim/DeleteMesaj/" + mesajId);
		}
	}
}
