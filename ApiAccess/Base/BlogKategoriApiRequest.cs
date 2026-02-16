using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
	public class BlogKategoriApiRequest : IBlogKategoriApiRequest
	{
		private readonly IRequestService _requestService;
		public BlogKategoriApiRequest(IRequestService requestService)
		{
			_requestService = requestService;
		}

		public List<BlogKategorilerDto> GetKategoriler() => _requestService.Get<List<BlogKategorilerDto>>("/BlogKategori/GetAllKategori");

		public List<BlogKategorilerDto> GetAktifKategoriler() => _requestService.Get<List<BlogKategorilerDto>>("/BlogKategori/GetAktifKategoriler");

		public BlogKategorilerDto GetKategoriById(int kategoriId) => _requestService.Get<BlogKategorilerDto>("/BlogKategori/GetKategoriById?kategoriId=" + kategoriId);

		public BlogKategorilerDto KategoriEkle(BlogKategorilerDto model)
		{
			return _requestService.Post<BlogKategorilerDto>("/BlogKategori/InsertKategori", model);
		}

		public BlogKategorilerDto UpdateKategori(BlogKategorilerDto model)
		{
			return _requestService.Put<BlogKategorilerDto>("/BlogKategori/UpdateKategori", model);
		}

		public bool DeleteKategori(int kategoriId)
		{
			return _requestService.Delete<bool>("/BlogKategori/DeleteKategori/" + kategoriId);
		}
	}
}
