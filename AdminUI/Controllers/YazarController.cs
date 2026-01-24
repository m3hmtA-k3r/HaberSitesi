using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace AdminUI.Controllers
{
	public class YazarController : Controller
	{
		private readonly IYazarApiRequest _yazarApiRequest;
		private readonly ICommonApiRequest _commonApiRequest;
		public YazarController(IYazarApiRequest yazarApiRequest, ICommonApiRequest commonApiRequest)
		{
			_yazarApiRequest = yazarApiRequest;
			_commonApiRequest = commonApiRequest;
		}
		public IActionResult Index() => View(_yazarApiRequest.GetAllYazar());

		public IActionResult Ekle()
		{
			return View();
		}

		public IActionResult YazarEkle(YazarViewModel model)
		{
			string resimUrl = "";
			if (model.ResimFile != null)
			{
				resimUrl = _commonApiRequest.Upload(model.ResimFile) ?? "";
			}

			YazarlarDto yazarlarDto = new YazarlarDto();
			yazarlarDto.Ad = model.Ad ?? "";
			yazarlarDto.Soyad = model.Soyad ?? "";
			yazarlarDto.Sifre = model.Sifre ?? "";
			yazarlarDto.Aktifmi = model.Aktifmi;
			yazarlarDto.Eposta = model.Eposta ?? "";
			yazarlarDto.Resim = resimUrl;
			var result = _yazarApiRequest.InsertYazar(yazarlarDto);
			return RedirectToAction("Index", "Yazar");
		}

		public IActionResult Guncelle(int yazarId)
		{
			var yazar = _yazarApiRequest.GetYazarById(yazarId);
			YazarViewModel model = new YazarViewModel();
			model.Id = yazar.Id;
			model.Ad = yazar.Ad;
			model.Soyad = yazar.Soyad;
			model.Sifre = yazar.Sifre;
			model.Aktifmi = yazar.Aktifmi;
			model.Eposta = yazar.Eposta;
			model.Resim = yazar.Resim;
			return View(model);
		}
		[HttpPost]
		public IActionResult YazarGuncelle(YazarViewModel model)
		{
			string resimUrl = model.Resim ?? "";
			if (model.ResimFile != null)
			{
				var uploadedResim = _commonApiRequest.Upload(model.ResimFile);
				if (!string.IsNullOrEmpty(uploadedResim))
				{
					resimUrl = uploadedResim;
				}
			}

			YazarlarDto yazarlarDto = new YazarlarDto();
			yazarlarDto.Id = model.Id;
			yazarlarDto.Ad = model.Ad ?? "";
			yazarlarDto.Soyad = model.Soyad ?? "";
			yazarlarDto.Sifre = model.Sifre ?? "";
			yazarlarDto.Aktifmi = model.Aktifmi;
			yazarlarDto.Eposta = model.Eposta ?? "";
			yazarlarDto.Resim = resimUrl;
			_yazarApiRequest.UpdateYazar(yazarlarDto);
			return RedirectToAction("Index", "Yazar");
		}

		public IActionResult Sil(int yazarId)
		{
			_yazarApiRequest.DeleteYazar(yazarId);
			return RedirectToAction("Index");
		}
	}
}
