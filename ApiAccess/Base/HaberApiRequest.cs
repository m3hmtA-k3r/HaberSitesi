using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class HaberApiRequest : IHaberApiRequest
    {
        private readonly IRequestService _requestService;
        public HaberApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }
        public List<HaberlerDto> GetAllHaber() => _requestService.Get<List<HaberlerDto>>("Haber/GetAllHaber");
		public HaberlerDto HaberEkle(HaberlerDto model)
		{
			return _requestService.Post<HaberlerDto>("/Haber/InsertHaber", model);
		}
		public HaberlerDto GetHaberById(int haberId) => _requestService.Get<HaberlerDto>("/Haber/GetHaberById?haberId=" + haberId);

		public HaberlerDto UpdateHaber(HaberlerDto model)
		{
			return _requestService.Post<HaberlerDto>("/haber/updatehaber", model);
		}

		public bool DeleteHaber(int haberId)
		{
			return _requestService.Get<bool>("/haber/deletehaber?haberId=" + haberId);
		}
	}
}

