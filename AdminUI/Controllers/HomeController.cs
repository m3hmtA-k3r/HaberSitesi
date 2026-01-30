using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace AdminUI.Controllers
{
	[Authorize]
	public class HomeController(
		IHaberApiRequest haberApiRequest,
		IYorumApiRequest yorumApiRequest,
		ISlaytApiRequest slaytApiRequest,
		IBlogApiRequest blogApiRequest,
		IBlogYorumApiRequest blogYorumApiRequest,
		IBlogKategoriApiRequest blogKategoriApiRequest,
		IKategoriApiRequest kategoriApiRequest,
		IYazarApiRequest yazarApiRequest,
		IKullaniciApiRequest kullaniciApiRequest) : Controller
	{
		private readonly IHaberApiRequest _haberApiRequest = haberApiRequest;
		private readonly IYorumApiRequest _yorumApiRequest = yorumApiRequest;
		private readonly ISlaytApiRequest _slaytApiRequest = slaytApiRequest;
		private readonly IBlogApiRequest _blogApiRequest = blogApiRequest;
		private readonly IBlogYorumApiRequest _blogYorumApiRequest = blogYorumApiRequest;
		private readonly IBlogKategoriApiRequest _blogKategoriApiRequest = blogKategoriApiRequest;
		private readonly IKategoriApiRequest _kategoriApiRequest = kategoriApiRequest;
		private readonly IYazarApiRequest _yazarApiRequest = yazarApiRequest;
		private readonly IKullaniciApiRequest _kullaniciApiRequest = kullaniciApiRequest;

		public IActionResult Index()
		{
			// Haber verileri
			var haberler = _haberApiRequest.GetAllHaber() ?? new List<HaberlerDto>();
			var toplamHaber = haberler.Count;
			var aktifHaber = haberler.Count(h => h.Aktifmi);

			// Blog verileri
			var bloglar = _blogApiRequest.GetBloglar() ?? new List<BloglarDto>();
			var toplamBlog = bloglar.Count;
			var aktifBlog = bloglar.Count(b => b.AktifMi);

			// Yorum verileri (haber)
			var bekleyenHaberYorum = _yorumApiRequest.GetOnayBekleyenYorumSayisi();
			var tumHaberYorumlar = _yorumApiRequest.GetAllYorum() ?? new List<YorumlarDto>();
			var onaylananHaberYorum = tumHaberYorumlar.Count(y => y.Aktifmi);

			// Yorum verileri (blog)
			var bekleyenBlogYorumlar = _blogYorumApiRequest.GetOnayBekleyenYorumlar() ?? new List<BlogYorumlarDto>();
			var bekleyenBlogYorum = bekleyenBlogYorumlar.Count;
			var tumBlogYorumlar = _blogYorumApiRequest.GetYorumlar() ?? new List<BlogYorumlarDto>();
			var onaylananBlogYorum = tumBlogYorumlar.Count(y => y.OnaylandiMi);

			// Slayt verileri
			var slaytlar = _slaytApiRequest.GetAllSlayt() ?? new List<SlaytlarDto>();
			var toplamSlayt = slaytlar.Count;
			var pasifSlayt = _slaytApiRequest.GetUnpublishedSlidesCount();

			// Kategori verileri
			var haberKategorileri = _kategoriApiRequest.GetKategoriler() ?? new List<KategorilerDto>();
			var blogKategorileri = _blogKategoriApiRequest.GetKategoriler() ?? new List<BlogKategorilerDto>();
			var toplamKategori = haberKategorileri.Count + blogKategorileri.Count;

			// Yazar verileri
			var yazarlar = _yazarApiRequest.GetAllYazar() ?? new List<YazarlarDto>();
			var toplamYazar = yazarlar.Count;

			// Kullanici verileri
			var kullanicilar = _kullaniciApiRequest.GetAllKullanici() ?? new List<KullaniciDto>();
			var toplamKullanici = kullanicilar.Count;

			// Son eklenen 5 haber
			var sonHaberler = haberler
				.OrderByDescending(h => h.EklenmeTarihi)
				.Take(5)
				.ToList();

			// Son eklenen 5 blog
			var sonBloglar = bloglar
				.OrderByDescending(b => b.OlusturmaTarihi)
				.Take(5)
				.ToList();

			// ViewBag - Ozet kartlar
			ViewBag.ToplamHaber = toplamHaber;
			ViewBag.AktifHaber = aktifHaber;
			ViewBag.ToplamBlog = toplamBlog;
			ViewBag.AktifBlog = aktifBlog;
			ViewBag.BekleyenYorum = bekleyenHaberYorum + bekleyenBlogYorum;
			ViewBag.BekleyenHaberYorum = bekleyenHaberYorum;
			ViewBag.BekleyenBlogYorum = bekleyenBlogYorum;
			ViewBag.ToplamKullanici = toplamKullanici;

			// ViewBag - Grafikler
			ViewBag.ToplamSlayt = toplamSlayt;
			ViewBag.ToplamKategori = toplamKategori;
			ViewBag.HaberKategoriSayisi = haberKategorileri.Count;
			ViewBag.BlogKategoriSayisi = blogKategorileri.Count;
			ViewBag.OnaylananHaberYorum = onaylananHaberYorum;
			ViewBag.OnaylananBlogYorum = onaylananBlogYorum;

			// ViewBag - Bilgi kartlar
			ViewBag.ToplamYazar = toplamYazar;
			ViewBag.PasifSlayt = pasifSlayt;

			// ViewBag - Son eklenenler
			ViewBag.SonHaberler = sonHaberler;
			ViewBag.SonBloglar = sonBloglar;

			return View();
		}
	}
}
