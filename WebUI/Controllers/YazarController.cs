using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class YazarController : Controller
    {
        private readonly IYazarApiRequest _yazarApiRequest;
        private readonly IHaberApiRequest _haberApiRequest;
        private readonly ILogger<YazarController> _logger;

        public YazarController(IYazarApiRequest yazarApiRequest, IHaberApiRequest haberApiRequest, ILogger<YazarController> logger)
        {
            _yazarApiRequest = yazarApiRequest;
            _haberApiRequest = haberApiRequest;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Profil(int id, int sayfa = 1)
        {
            try
            {
                const int sayfaBoyutu = 6;

                if (id <= 0)
                    return RedirectToAction("Index", "Haberler");

                var yazar = _yazarApiRequest.GetYazarById(id);
                if (yazar == null || !yazar.Aktifmi)
                    return RedirectToAction("Index", "Haberler");

                var tumHaberler = _haberApiRequest.GetAllHaber();
                var yazarHaberleri = tumHaberler?
                    .Where(x => x.Aktifmi && x.YazarId == id)
                    .OrderByDescending(x => x.EklenmeTarihi)
                    .ToList() ?? new List<HaberlerDto>();

                var paginationInfo = PaginationHelper.GetPaginationInfo(yazarHaberleri.Count, sayfa, sayfaBoyutu);
                var sayfaliHaberler = PaginationHelper.GetPaginatedList(yazarHaberleri, sayfa, sayfaBoyutu);

                var model = new YazarProfilViewModel
                {
                    Yazar = yazar,
                    Haberler = sayfaliHaberler,
                    ToplamHaberSayisi = yazarHaberleri.Count,
                    ToplamGoruntulenme = yazarHaberleri.Sum(x => x.GosterimSayisi),
                    Pagination = paginationInfo
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazar profili yuklenirken hata olustu: {YazarId}", id);
                return RedirectToAction("Index", "Haberler");
            }
        }
    }
}
