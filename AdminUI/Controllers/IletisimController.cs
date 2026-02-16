using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminUI.Controllers
{
	[Authorize]
	public class IletisimController : Controller
	{
		private readonly IIletisimApiRequest _iletisimApiRequest;

		public IletisimController(IIletisimApiRequest iletisimApiRequest)
		{
			_iletisimApiRequest = iletisimApiRequest;
		}

		public IActionResult Index()
		{
			var mesajlar = _iletisimApiRequest.GetAllMesaj();
			return View(mesajlar);
		}

		public IActionResult Detay(int mesajId)
		{
			var mesaj = _iletisimApiRequest.GetMesajById(mesajId);
			if (!mesaj.OkunduMu)
			{
				_iletisimApiRequest.OkunduOlarakIsaretle(mesajId);
				mesaj.OkunduMu = true;
			}
			return View(mesaj);
		}

		[HttpPost]
		public IActionResult OkunduIsaretle(int mesajId)
		{
			_iletisimApiRequest.OkunduOlarakIsaretle(mesajId);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult CevaplandiIsaretle(int mesajId)
		{
			_iletisimApiRequest.CevaplandiOlarakIsaretle(mesajId);
			return RedirectToAction("Detay", new { mesajId });
		}

		public IActionResult Sil(int mesajId)
		{
			_iletisimApiRequest.DeleteMesaj(mesajId);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult SilAjax(int mesajId)
		{
			try
			{
				_iletisimApiRequest.DeleteMesaj(mesajId);
				return Json(new { success = true, message = "Mesaj silindi." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}
	}
}
