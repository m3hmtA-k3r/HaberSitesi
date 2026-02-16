using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HaberlerController : Controller
    {
        private readonly IHaberApiRequest _haberApiRequest;
        private readonly IKategoriApiRequest _kategoriApiRequest;
        private readonly IYorumApiRequest _yorumApiRequest;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HaberlerController> _logger;

        public HaberlerController(IHaberApiRequest haberApiRequest, IKategoriApiRequest kategoriApiRequest, IYorumApiRequest yorumApiRequest, IMemoryCache cache, ILogger<HaberlerController> logger)
        {
            _haberApiRequest = haberApiRequest;
            _kategoriApiRequest = kategoriApiRequest;
            _yorumApiRequest = yorumApiRequest;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int? kategoriId = null, int sayfa = 1, string siralama = "yeni")
        {
            try
            {
                const int sayfaBoyutu = 9;

                var pagedResult = _haberApiRequest.GetHaberlerPaged(sayfa, sayfaBoyutu, aktif: true, kategoriId: kategoriId, siralama: siralama);
                var kategoriler = _cache.GetOrCreate("haber_kategoriler", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _kategoriApiRequest.GetKategoriler() ?? new List<KategorilerDto>();
                })!;

                var paginationInfo = new PaginationInfo
                {
                    CurrentPage = pagedResult?.Page ?? 1,
                    TotalPages = pagedResult?.TotalPages ?? 0,
                    PageSize = sayfaBoyutu,
                    TotalItems = pagedResult?.TotalCount ?? 0
                };

                var model = new HaberlerViewModel
                {
                    Haberler = pagedResult?.Items ?? new List<HaberlerDto>(),
                    Kategoriler = kategoriler.Where(x => x.Aktifmi).OrderByDescending(x => x.Id).ToList(),
                    Pagination = paginationInfo,
                    SeciliKategoriId = kategoriId,
                    SiralamaSecimi = siralama
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haberler listesi yuklenirken hata olustu");
                return View(new HaberlerViewModel());
            }
        }

        [HttpGet]
        public IActionResult Detay(int id)
        {
            try
            {
                if (id <= 0)
                    return RedirectToAction("Index");

                var haber = _haberApiRequest.GetHaberById(id);
                if (haber == null || !haber.Aktifmi)
                    return RedirectToAction("Index");

                // Gosterim sayisini artir
                haber.GosterimSayisi++;
                try { _haberApiRequest.UpdateHaber(haber); }
                catch (Exception ex) { _logger.LogWarning(ex, "Gosterim sayisi guncellenemedi: Haber {HaberId}", id); }

                var yorumlar = _yorumApiRequest.GetAllYorum();
                var kategoriler = _cache.GetOrCreate("haber_kategoriler", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _kategoriApiRequest.GetKategoriler();
                });

                var ilgiliResult = _haberApiRequest.GetHaberlerPaged(1, 5, aktif: true, kategoriId: haber.KategoriId);
                var ilgiliHaberler = ilgiliResult?.Items?
                    .Where(x => x.Id != haber.Id)
                    .Take(4)
                    .ToList() ?? new List<HaberlerDto>();

                var model = new HaberDetayViewModel
                {
                    Haber = haber,
                    Yorumlar = yorumlar?.Where(x => x.HaberId == id && x.Aktifmi)
                        .OrderByDescending(x => x.EklenmeTarihi).ToList() ?? new List<YorumlarDto>(),
                    Kategoriler = kategoriler?.Where(x => x.Aktifmi).OrderByDescending(x => x.Id).ToList() ?? new List<KategorilerDto>(),
                    IlgiliHaberler = ilgiliHaberler
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber detayi yuklenirken hata olustu: {HaberId}", id);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YorumYap(YorumlarDto model)
        {
            if (model == null || model.HaberId <= 0)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(model.Ad) ||
                string.IsNullOrWhiteSpace(model.Soyad) ||
                string.IsNullOrWhiteSpace(model.Icerik))
            {
                return RedirectToAction("Detay", new { id = model.HaberId });
            }

            try
            {
                var result = _yorumApiRequest.InsertYorum(model);
                if (result == null)
                    return RedirectToAction("Index");
                return RedirectToAction("Detay", new { id = result.HaberId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum gonderilemedi: Haber {HaberId}", model.HaberId);
                return RedirectToAction("Detay", new { id = model.HaberId });
            }
        }
    }
}
