using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SearchController : Controller
    {
        private readonly IHaberApiRequest _haberApiRequest;
        private readonly IKategoriApiRequest _kategoriApiRequest;

        public SearchController(IHaberApiRequest haberApiRequest, IKategoriApiRequest kategoriApiRequest)
        {
            _haberApiRequest = haberApiRequest;
            _kategoriApiRequest = kategoriApiRequest;
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
            {
                return View(model);
            }

            var allHaberler = _haberApiRequest.GetAllHaber();
            var kategoriler = _kategoriApiRequest.GetKategoriler();

            if (allHaberler != null)
            {
                var searchTerm = q.ToLower().Trim();
                model.Haberler = allHaberler
                    .Where(x => x.Aktifmi &&
                        (x.Baslik.ToLower().Contains(searchTerm) ||
                         x.Icerik.ToLower().Contains(searchTerm) ||
                         x.Kategori.ToLower().Contains(searchTerm) ||
                         x.Yazar.ToLower().Contains(searchTerm)))
                    .OrderByDescending(x => x.EklenmeTarihi)
                    .ToList();
            }

            if (kategoriler != null)
            {
                model.Kategoriler = kategoriler.Where(x => x.Aktifmi).ToList();
            }

            return View(model);
        }
    }
}
