using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HaberlerController : Controller
    {
        private readonly IHaberApiRequest _haberApiRequest;
        private readonly IKategoriApiRequest _kategoriApiRequest;
        private readonly IYorumApiRequest _yorumApiRequest;

        public HaberlerController(IHaberApiRequest haberApiRequest, IKategoriApiRequest kategoriApiRequest, IYorumApiRequest yorumApiRequest)
        {
            _haberApiRequest = haberApiRequest;
            _kategoriApiRequest = kategoriApiRequest;
            _yorumApiRequest = yorumApiRequest;
        }

        [HttpGet]
        public IActionResult Index(int id = 0)
        {
            var haberler = _haberApiRequest.GetAllHaber();
            var kategoriler = _kategoriApiRequest.GetKategoriler();

            if (haberler == null)
                haberler = new List<HaberlerDto>();

            if (kategoriler == null)
                kategoriler = new List<KategorilerDto>();

            // Sadece aktif haberleri filtrele
            var aktifHaberler = haberler.Where(x => x.Aktifmi);

            var model = new HaberlerViewModel
            {
                Haberler = id == 0
                    ? aktifHaberler.OrderByDescending(x => x.EklenmeTarihi).ToList()
                    : aktifHaberler.Where(x => x.KategoriId == id).OrderByDescending(x => x.EklenmeTarihi).ToList(),
                Kategoriler = kategoriler.Where(x => x.Aktifmi).OrderByDescending(x => x.Id).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Detay(int id)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var haber = _haberApiRequest.GetHaberById(id);
            if (haber == null || !haber.Aktifmi)
                return RedirectToAction("Index");

            // Gösterim sayısını artır
            haber.GosterimSayisi++;
            _haberApiRequest.UpdateHaber(haber);

            var yorumlar = _yorumApiRequest.GetAllYorum();
            var kategoriler = _kategoriApiRequest.GetKategoriler();

            var model = new HaberDetayViewModel
            {
                Haber = haber,
                Yorumlar = yorumlar?.Where(x => x.HaberId == id && x.Aktifmi)
                    .OrderByDescending(x => x.EklenmeTarihi).ToList() ?? new List<YorumlarDto>(),
                Kategoriler = kategoriler?.Where(x => x.Aktifmi).OrderByDescending(x => x.Id).ToList() ?? new List<KategorilerDto>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YorumYap(YorumlarDto model)
        {
            if (model == null || model.HaberId <= 0)
                return RedirectToAction("Index");

            // Temel validasyon
            if (string.IsNullOrWhiteSpace(model.Ad) ||
                string.IsNullOrWhiteSpace(model.Soyad) ||
                string.IsNullOrWhiteSpace(model.Icerik))
            {
                return RedirectToAction("Detay", new { id = model.HaberId });
            }

            var result = _yorumApiRequest.InsertYorum(model);

            if (result == null)
                return RedirectToAction("Index");

            return RedirectToAction("Detay", new { id = result.HaberId });
        }
    }
}
