using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class YorumApiRequest : IYorumApiRequest
    {
        private readonly IRequestService _requestService;
        public YorumApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }
        public bool DeleteYorum(int yorumId)
        {
            return _requestService.Get<bool>("Yorum/DeleteYorum?yorumId=" + yorumId);
        }

        public List<YorumlarDto> GetAllYorum()
        {
            return _requestService.Get<List<YorumlarDto>>("Yorum/GetAllYorum");
        }

        public YorumlarDto GetYorumById(int yorumId) => _requestService.Get<YorumlarDto>("Yorum/GetYorumById?yorumId=" + yorumId);


        public YorumlarDto InsertYorum(YorumlarDto model)
        {
            return _requestService.Post<YorumlarDto>("Yorum/InsertYorum", model);
        }

        public YorumlarDto UpdateYorum(YorumlarDto model)
        {
            return _requestService.Post<YorumlarDto>("Yorum/UpdateYorum", model);
        }
    }
}
