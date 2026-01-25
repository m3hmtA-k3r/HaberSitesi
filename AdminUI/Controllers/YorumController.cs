using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Reflection;

namespace AdminUI.Controllers
{
	[Authorize]
	public class YorumController : Controller
	{
		private readonly IYorumApiRequest _yorumRequest;
		public YorumController(IYorumApiRequest yorumRequest)
		{
			_yorumRequest = yorumRequest;
		}
		public IActionResult Index()
		{
			List<YorumlarDto> pageData = _yorumRequest.GetAllYorum();

			return View(pageData);
		}
		public IActionResult Ekle()
		{
			return View();
		}

		[HttpPost]
		public IActionResult YorumEkle(YorumViewModel model)
		{
			YorumlarDto yorum = new YorumlarDto();
			yorum.Ad = model.Ad;
			yorum.Soyad = model.Soyad;
			yorum.Eposta = model.Eposta;
			yorum.Baslik = model.Baslik;
			yorum.Icerik = model.Icerik;
			yorum.HaberId = model.HaberId;
			yorum.Aktifmi = model.Aktifmi;
			var insertedData = _yorumRequest.InsertYorum(yorum);
			return RedirectToAction("Index");
		}
		public IActionResult Guncelle(int yorumId)
		{
			var model = _yorumRequest.GetYorumById(yorumId);
			YorumViewModel yorum = new YorumViewModel();
			yorum.Id = model.Id;
			yorum.Ad = model.Ad;
			yorum.Soyad = model.Soyad;
			yorum.Eposta = model.Eposta;
			yorum.Baslik = model.Baslik;
			yorum.Icerik = model.Icerik;
			yorum.HaberId = model.HaberId;
			yorum.Aktifmi = model.Aktifmi;
			return View(yorum);
		}

		[HttpPost]
		public IActionResult YorumGuncelle(YorumViewModel model)
		{
			YorumlarDto yorum = new YorumlarDto();
			yorum.Id = model.Id;
			yorum.Ad = model.Ad;
			yorum.Soyad = model.Soyad;
			yorum.Eposta = model.Eposta;
			yorum.Baslik = model.Baslik;
			yorum.Icerik = model.Icerik;
			yorum.HaberId = model.HaberId;
			yorum.Aktifmi = model.Aktifmi;
			var insertedData = _yorumRequest.UpdateYorum(yorum);
			return RedirectToAction("Index");
		}

		public IActionResult Sil(int yorumId)
		{
			_yorumRequest.DeleteYorum(yorumId);
			return RedirectToAction("Index");
		}
	}
}
