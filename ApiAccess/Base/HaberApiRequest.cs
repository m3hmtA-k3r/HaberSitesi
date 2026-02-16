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

		public PagedResultDto<HaberlerDto> GetHaberlerPaged(int sayfa = 1, int boyut = 9, bool? aktif = null, int? kategoriId = null, string siralama = "yeni")
		{
			var url = $"Haber/GetHaberlerPaged?sayfa={sayfa}&boyut={boyut}&siralama={siralama}";
			if (aktif.HasValue)
				url += $"&aktif={aktif.Value}";
			if (kategoriId.HasValue && kategoriId.Value > 0)
				url += $"&kategoriId={kategoriId.Value}";
			return _requestService.Get<PagedResultDto<HaberlerDto>>(url);
		}

		public HaberlerDto HaberEkle(HaberlerDto model)
		{
			return _requestService.Post<HaberlerDto>("/Haber/InsertHaber", model);
		}
		public HaberlerDto GetHaberById(int haberId) => _requestService.Get<HaberlerDto>("/Haber/GetHaberById?haberId=" + haberId);

		public HaberlerDto UpdateHaber(HaberlerDto model)
		{
			return _requestService.Put<HaberlerDto>("/haber/updatehaber", model);
		}

		public bool DeleteHaber(int haberId)
		{
			return _requestService.Delete<bool>("/haber/deletehaber/" + haberId);
		}
	}
}

