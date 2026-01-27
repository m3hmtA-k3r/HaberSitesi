using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace AdminUI.Controllers
{
    [Authorize]
    public class KategoriController : Controller
    {
        private readonly IKategoriApiRequest _kategoriApiRequest;
        public KategoriController(IKategoriApiRequest kategoriApiRequest)
        {
            _kategoriApiRequest = kategoriApiRequest;
        }
        public IActionResult Index()
        {
            var pageData = _kategoriApiRequest.GetKategoriler();

			return View(pageData);
		}
		public IActionResult Ekle()
		{
			return View();
		}

		[HttpPost]
		public IActionResult KategoriEkle(KategoriViewModel model)
		{
			KategorilerDto kategori = new KategorilerDto();
			kategori.Aktifmi = model.Aktifmi;
			kategori.Aciklama = model.Aciklama;
			var result = _kategoriApiRequest.KategoriEkle(kategori);

			return RedirectToAction("Index");
		}
		public IActionResult Guncelle(int kategoriId)
		{
			var data = _kategoriApiRequest.GetKategoriById(kategoriId);

			KategoriViewModel model = new KategoriViewModel();
			model.Aktifmi = data.Aktifmi;
			model.Id = data.Id;
			model.Aciklama = data.Aciklama;

			return View(model);
		}

		[HttpPost]
		public IActionResult KategoriGuncelle(KategoriViewModel model)
		{
			KategorilerDto kategori = new KategorilerDto();
			kategori.Id = model.Id.Value;
			kategori.Aktifmi = model.Aktifmi;
			kategori.Aciklama = model.Aciklama;
			_kategoriApiRequest.UpdateKategori(kategori);
			return RedirectToAction("Index");
		}

		public IActionResult Sil(int kategoriId)
		{
			_kategoriApiRequest.DeleteKategori(kategoriId);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult SilAjax(int kategoriId)
		{
			try
			{
				_kategoriApiRequest.DeleteKategori(kategoriId);
				return Json(new { success = true, message = "Kategori silindi." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}


	}
}
