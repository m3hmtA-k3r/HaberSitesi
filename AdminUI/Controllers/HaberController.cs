using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Dtos;
using System.Reflection.Emit;

namespace AdminUI.Controllers
{
	public class HaberController : Controller
	{
		private readonly IHaberApiRequest _haberApiRequest;
		private readonly ICommonApiRequest _commonApiRequest;
		private readonly IYazarApiRequest _yazarApiRequest;
		private readonly IKategoriApiRequest _kategoriApiRequest;
		public HaberController(IHaberApiRequest haberApiRequest, ICommonApiRequest commonApiRequest, IYazarApiRequest yazarApiRequest, IKategoriApiRequest kategoriApiRequest)
		{
			_haberApiRequest = haberApiRequest;
			_commonApiRequest = commonApiRequest;
			_yazarApiRequest = yazarApiRequest;
			_kategoriApiRequest = kategoriApiRequest;
		}
		public IActionResult Index()
		{
			var pageData = _haberApiRequest.GetAllHaber();

			return View(pageData);
		}
		public IActionResult Ekle()
		{
			var yazarlar = _yazarApiRequest.GetAllYazar().Select(x=> new SelectListItem { Text = x.Ad + " " + x.Soyad, Value = x.Id.ToString() }).ToList();
			var kategoriler = _kategoriApiRequest.GetKategoriler().Select(x => new SelectListItem { Text = x.Aciklama, Value = x.Id.ToString() }).ToList();
			HaberViewModel model = new HaberViewModel();
			model.Yazarlar = yazarlar;
			model.Kategoriler = kategoriler;
			return View(model);
		}

		[HttpPost]
		public IActionResult HaberEkle(HaberViewModel model)
		{
			string resimUrl = model.Resim;
			if (model.ResimFile != null)
			{
				resimUrl = _commonApiRequest.Upload(model.ResimFile);
			}
			string videoUrl = model.Video;
			if (model.VideoFile != null)
			{
				videoUrl = _commonApiRequest.Upload(model.VideoFile);
			}
			HaberlerDto haber = new HaberlerDto();
			haber.YazarId = model.YazarId;
			haber.GosterimSayisi = model.GosterimSayisi;
			haber.Resim = resimUrl;
			haber.Aktifmi = model.Aktifmi;
			haber.Baslik = model.Baslik;
			haber.KategoriId = model.KategoriId;
			haber.Icerik = model.Icerik;
			haber.Video = videoUrl;

			var result = _haberApiRequest.HaberEkle(haber);

			return RedirectToAction("Index");
		}
		public IActionResult Guncelle(int haberId)
		{
			var yazarlar = _yazarApiRequest.GetAllYazar().Select(x => new SelectListItem { Text = x.Ad + " " + x.Soyad, Value = x.Id.ToString() }).ToList();
			var kategoriler = _kategoriApiRequest.GetKategoriler().Select(x => new SelectListItem { Text = x.Aciklama, Value = x.Id.ToString() }).ToList();
			var model = _haberApiRequest.GetHaberById(haberId);

			HaberViewModel haber = new HaberViewModel();
			haber.Id = model.Id;
			haber.EklenmeTarihi = model.EklenmeTarihi;
			haber.YazarId = model.YazarId;
			haber.GosterimSayisi = model.GosterimSayisi;
			haber.Resim = model.Resim;
			haber.Aktifmi = model.Aktifmi;
			haber.Baslik = model.Baslik;
			haber.KategoriId = model.KategoriId;
			haber.Icerik = model.Icerik;
			haber.Video = model.Video;
			haber.Yazarlar = yazarlar;
			haber.Kategoriler = kategoriler;

			return View(haber);
		}

		[HttpPost]
		public IActionResult HaberGuncelle(HaberViewModel model)
		{
			string resimUrl = model.Resim;
			if (model.ResimFile != null)
			{
				resimUrl = _commonApiRequest.Upload(model.ResimFile);
			}
			string videoUrl = model.Video;
			if (model.VideoFile != null)
			{
				videoUrl = _commonApiRequest.Upload(model.VideoFile);
			}
			HaberlerDto haber = new HaberlerDto();
			haber.Id = model.Id;
			haber.YazarId = model.YazarId;
			haber.GosterimSayisi = model.GosterimSayisi;
			haber.Resim = resimUrl;
			haber.Aktifmi = model.Aktifmi;
			haber.Baslik = model.Baslik;
			haber.KategoriId = model.KategoriId;
			haber.Icerik = model.Icerik;
			haber.Video = videoUrl;
			_haberApiRequest.UpdateHaber(haber);
			return RedirectToAction("Index");
		}

		public IActionResult Sil(int haberId)
		{
			_haberApiRequest.DeleteHaber(haberId);
			return RedirectToAction("Index");
		}


	}
}
