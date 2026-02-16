using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class YazarApiRequest : IYazarApiRequest
    {
        private readonly IRequestService _requestService;
        public YazarApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }
        public YazarlarDto GetYazarByEmailPassword(string email, string password)
        {
            return _requestService.Get<YazarlarDto>("Yazar/GetYazarByEmailPassword?email=" + email + "&password=" + password);
		}
		public bool DeleteYazar(int yazarId)
		{
			return _requestService.Delete<bool>("Yazar/DeleteYazar/" + yazarId);
		}

		public List<YazarlarDto> GetAllYazar()
		{
			return _requestService.Get<List<YazarlarDto>>("Yazar/GetAllYazar");
		}

		public YazarlarDto GetYazarById(int yazarId) => _requestService.Get<YazarlarDto>("Yazar/GetYazarById?yazarId=" + yazarId);


		public YazarlarDto InsertYazar(YazarlarDto model)
		{
			return _requestService.Post<YazarlarDto>("Yazar/InsertYazar", model);
		}

		public YazarlarDto UpdateYazar(YazarlarDto model)
		{
			return _requestService.Put<YazarlarDto>("Yazar/UpdateYazar", model);
		}
	}
}
