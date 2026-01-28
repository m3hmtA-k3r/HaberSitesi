using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class YazarController : Controller
    {
        private readonly IYazarApiRequest _yazarApiRequest;
        private readonly IHaberApiRequest _haberApiRequest;

        public YazarController(IYazarApiRequest yazarApiRequest, IHaberApiRequest haberApiRequest)
        {
            _yazarApiRequest = yazarApiRequest;
            _haberApiRequest = haberApiRequest;
        }

        [HttpGet]
        public IActionResult Profil(int id, int sayfa = 1)
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

            // Pagination
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
    }
}
