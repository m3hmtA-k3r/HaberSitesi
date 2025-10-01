using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Entities;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HaberlerController : Controller
    {
        private readonly IHaberApiRequest haberApiRequest;
        private readonly IKategoriApiRequest kategoriApiRequest;
        private readonly IYorumApiRequest yorumApiRequest;
        public HaberlerController(IHaberApiRequest haberApiRequest, IKategoriApiRequest kategoriApiRequest, IYorumApiRequest yorumApiRequest)
        {
            this.haberApiRequest = haberApiRequest;
            this.kategoriApiRequest = kategoriApiRequest;
            this.yorumApiRequest = yorumApiRequest;
        }
        public IActionResult Index(int id)
        {
			HaberlerViewModel model = new HaberlerViewModel();
            if (id == 0)
                model.Haberler = haberApiRequest.GetAllHaber().OrderByDescending(x => x.EklenmeTarihi).ToList();
            else
                model.Haberler = haberApiRequest.GetAllHaber().Where(x => x.KategoriId == id).OrderByDescending(x => x.EklenmeTarihi).ToList();

            model.Kategoriler = kategoriApiRequest.GetKategoriler().OrderByDescending(x=>x.Id).ToList();

			return View(model);
        }

        public IActionResult Detay(int id)
        {
            if (id == 0)
                RedirectToAction("Index");

            HaberDetayViewModel model = new HaberDetayViewModel();
            model.Haber = haberApiRequest.GetHaberById(id);
            model.Yorumlar = yorumApiRequest.GetAllYorum().Where(x => x.HaberId == id && x.Aktifmi).OrderByDescending(x => x.EklenmeTarihi).ToList();
            model.Kategoriler = kategoriApiRequest.GetKategoriler().OrderByDescending(x => x.Id).ToList();

            if (model.Haber == null)
                RedirectToAction("Index");

            return View(model);
        }
        public IActionResult YorumYap(YorumlarDto model)
        {
            var result = yorumApiRequest.InsertYorum(model);

            return Redirect("/Haberler/Detay/"+ result.HaberId);
        }
    }
}
