using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SearchController : Controller
    {
        private readonly IHaberApiRequest _haberApiRequest;
        private readonly IKategoriApiRequest _kategoriApiRequest;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IHaberApiRequest haberApiRequest, IKategoriApiRequest kategoriApiRequest, IMemoryCache cache, ILogger<SearchController> logger)
        {
            _haberApiRequest = haberApiRequest;
            _kategoriApiRequest = kategoriApiRequest;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string q)
        {
            var model = new SearchViewModel
            {
                Keyword = q ?? "",
                Haberler = new List<HaberlerDto>(),
                Kategoriler = new List<KategorilerDto>()
            };

            if (string.IsNullOrWhiteSpace(q))
                return View(model);

            try
            {
                var allHaberler = _haberApiRequest.GetAllHaber();
                var kategoriler = _cache.GetOrCreate("haber_kategoriler", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _kategoriApiRequest.GetKategoriler();
                });

                if (allHaberler != null)
                {
                    var searchTerm = q.ToLower().Trim();
                    model.Haberler = allHaberler
                        .Where(x => x.Aktifmi &&
                            (x.Baslik.ToLower().Contains(searchTerm) ||
                             x.Icerik.ToLower().Contains(searchTerm) ||
                             (x.Kategori ?? "").ToLower().Contains(searchTerm) ||
                             (x.Yazar ?? "").ToLower().Contains(searchTerm)))
                        .OrderByDescending(x => x.EklenmeTarihi)
                        .ToList();
                }

                if (kategoriler != null)
                    model.Kategoriler = kategoriler.Where(x => x.Aktifmi).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Arama sirasinda hata olustu: {Query}", q);
            }

            return View(model);
        }
    }
}
