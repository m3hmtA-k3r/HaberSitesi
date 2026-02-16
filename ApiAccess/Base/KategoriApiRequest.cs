using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class KategoriApiRequest : IKategoriApiRequest
    {
        private readonly IRequestService _requestService;
        public KategoriApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }

		public List<KategorilerDto> GetKategoriler() => _requestService.Get<List<KategorilerDto>>("/Kategori/GetAllKategori");

		public KategorilerDto KategoriEkle(KategorilerDto model)
		{
           return _requestService.Post<KategorilerDto>("/Kategori/InsertKategori", model);
		}
		public KategorilerDto GetKategoriById(int kategoriId) => _requestService.Get<KategorilerDto>("/Kategori/GetKategoriById?kategoriId=" + kategoriId);

		public KategorilerDto UpdateKategori(KategorilerDto model)
		{
			return _requestService.Put<KategorilerDto>("/kategori/updatekategori", model);
		}

		public bool DeleteKategori(int kategoriId)
		{
			return _requestService.Delete<bool>("/kategori/deletekategori/" + kategoriId);
		}
	}
}
