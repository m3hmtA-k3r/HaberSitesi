using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace AdminUI.Controllers
{
	[Authorize]
	public class BlogYorumController : Controller
	{
		private readonly IBlogYorumApiRequest _blogYorumApiRequest;

		public BlogYorumController(IBlogYorumApiRequest blogYorumApiRequest)
		{
			_blogYorumApiRequest = blogYorumApiRequest;
		}

		public IActionResult Index()
		{
			var pageData = _blogYorumApiRequest.GetYorumlar();
			return View(pageData);
		}

		public IActionResult OnayBekleyenler()
		{
			var pageData = _blogYorumApiRequest.GetOnayBekleyenYorumlar();
			return View("Index", pageData);
		}

		public IActionResult Guncelle(int yorumId)
		{
			var data = _blogYorumApiRequest.GetYorumById(yorumId);
			if (data == null)
				return RedirectToAction("Index");

			var model = new BlogYorumViewModel
			{
				Id = data.Id,
				BlogId = data.BlogId,
				BlogBaslik = data.BlogBaslik,
				Ad = data.Ad,
				Soyad = data.Soyad,
				Eposta = data.Eposta,
				Yorum = data.Yorum,
				OnaylandiMi = data.OnaylandiMi,
				AktifMi = data.AktifMi,
				OlusturmaTarihi = data.OlusturmaTarihi
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult YorumGuncelle(BlogYorumViewModel model)
		{
			var yorum = new BlogYorumlarDto
			{
				Id = model.Id,
				BlogId = model.BlogId,
				Ad = model.Ad,
				Soyad = model.Soyad,
				Eposta = model.Eposta,
				Yorum = model.Yorum,
				OnaylandiMi = model.OnaylandiMi,
				AktifMi = model.AktifMi
			};
			_blogYorumApiRequest.UpdateYorum(yorum);
			return RedirectToAction("Index");
		}

		public IActionResult Onayla(int yorumId)
		{
			_blogYorumApiRequest.OnaylaYorum(yorumId);
			return RedirectToAction("Index");
		}

		public IActionResult Reddet(int yorumId)
		{
			_blogYorumApiRequest.ReddetYorum(yorumId);
			return RedirectToAction("Index");
		}

		public IActionResult Sil(int yorumId)
		{
			_blogYorumApiRequest.DeleteYorum(yorumId);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult SilAjax(int yorumId)
		{
			try
			{
				_blogYorumApiRequest.DeleteYorum(yorumId);
				return Json(new { success = true, message = "Blog yorumu silindi." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}
	}
}
