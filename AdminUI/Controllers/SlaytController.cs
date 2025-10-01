using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Dtos;

namespace AdminUI.Controllers
{
	public class SlaytController : Controller
	{
		private readonly ISlaytApiRequest _slaytApiRequest;
		private readonly ICommonApiRequest _commonApiRequest;
		private readonly IHaberApiRequest _haberApiRequest;
		public SlaytController(ISlaytApiRequest slaytApiRequest, ICommonApiRequest commonApiRequest, IHaberApiRequest haberApiRequest)
		{
			_slaytApiRequest = slaytApiRequest;
			_commonApiRequest = commonApiRequest;
			_haberApiRequest = haberApiRequest;
		}
		public IActionResult Index() => View(_slaytApiRequest.GetAllSlayt());

		public IActionResult Ekle()
		{
			var haberler = _haberApiRequest.GetAllHaber().Select(x => new SelectListItem { Text = x.Baslik, Value = x.Id.ToString() }).ToList();
			SlaytViewModel model = new SlaytViewModel();
			model.Haberler = haberler;
			return View(model);
		}

		public IActionResult SlaytEkle(SlaytViewModel model)
		{
			var resimUrl = _commonApiRequest.Upload(model.ResimFile);

			SlaytlarDto slaytlarDto = new SlaytlarDto();
			slaytlarDto.Baslik = model.Baslik;
			slaytlarDto.Icerik = model.Icerik;
			slaytlarDto.HaberId = model.HaberId;
			slaytlarDto.Aktifmi = model.Aktifmi;
			slaytlarDto.Resim = resimUrl;
			var result = _slaytApiRequest.InsertSlayt(slaytlarDto);
			return RedirectToAction("Index", "Slayt");
		}

		public IActionResult Guncelle(int slaytId)
		{
			var haberler = _haberApiRequest.GetAllHaber().Select(x => new SelectListItem { Text = x.Baslik, Value = x.Id.ToString() }).ToList();
			var slayt = _slaytApiRequest.GetSlaytById(slaytId);
			SlaytViewModel model = new SlaytViewModel();
			model.Id = slayt.Id;
			model.Baslik = slayt.Baslik;
			model.Icerik = slayt.Icerik;
			model.HaberId = slayt.HaberId;
			model.Aktifmi = slayt.Aktifmi;
			model.Resim = slayt.Resim;
			model.Haberler = haberler;
			return View(model);
		}
		[HttpPost]
		public IActionResult SlaytGuncelle(SlaytViewModel model)
		{
			string resimUrl = model.Resim;
			if (model.ResimFile != null)
			{
				resimUrl = _commonApiRequest.Upload(model.ResimFile);
			}

			SlaytlarDto slaytlarDto = new SlaytlarDto();
			slaytlarDto.Id = model.Id;
			slaytlarDto.Baslik = model.Baslik;
			slaytlarDto.Icerik = model.Icerik;
			slaytlarDto.HaberId = model.HaberId;
			slaytlarDto.Aktifmi = model.Aktifmi;
			slaytlarDto.Resim = resimUrl;
			_slaytApiRequest.UpdateSlayt(slaytlarDto);
			return RedirectToAction("Index", "Slayt");
		}

		public IActionResult Sil(int slaytId)
		{
			_slaytApiRequest.DeleteSlayt(slaytId);
			return RedirectToAction("Index");
		}
	}
}
