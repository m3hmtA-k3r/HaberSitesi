using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Dtos;

namespace AdminUI.Controllers
{
	[Authorize]
	public class BlogController : Controller
	{
		private readonly IBlogApiRequest _blogApiRequest;
		private readonly IBlogKategoriApiRequest _blogKategoriApiRequest;
		private readonly IKullaniciApiRequest _kullaniciApiRequest;

		public BlogController(IBlogApiRequest blogApiRequest, IBlogKategoriApiRequest blogKategoriApiRequest, IKullaniciApiRequest kullaniciApiRequest)
		{
			_blogApiRequest = blogApiRequest;
			_blogKategoriApiRequest = blogKategoriApiRequest;
			_kullaniciApiRequest = kullaniciApiRequest;
		}

		public IActionResult Index()
		{
			var pageData = _blogApiRequest.GetBloglar();
			return View(pageData);
		}

		public IActionResult Ekle()
		{
			var model = new BlogViewModel
			{
				YayinTarihi = DateTime.Now,
				AktifMi = true,
				Kategoriler = GetKategorilerSelectList(),
				Yazarlar = GetYazarlarSelectList()
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult BlogEkle(BlogViewModel model)
		{
			var blog = new BloglarDto
			{
				Baslik = model.Baslik,
				Ozet = model.Ozet,
				Icerik = model.Icerik,
				GorselUrl = model.GorselUrl,
				Etiketler = model.Etiketler,
				YayinTarihi = model.YayinTarihi,
				AktifMi = model.AktifMi,
				KategoriId = model.KategoriId,
				YazarId = model.YazarId
			};
			_blogApiRequest.BlogEkle(blog);
			return RedirectToAction("Index");
		}

		public IActionResult Guncelle(int blogId)
		{
			var data = _blogApiRequest.GetBlogById(blogId);
			if (data == null)
				return RedirectToAction("Index");

			var model = new BlogViewModel
			{
				Id = data.Id,
				Baslik = data.Baslik,
				Ozet = data.Ozet,
				Icerik = data.Icerik,
				GorselUrl = data.GorselUrl,
				Etiketler = data.Etiketler,
				YayinTarihi = data.YayinTarihi,
				OlusturmaTarihi = data.OlusturmaTarihi,
				AktifMi = data.AktifMi,
				KategoriId = data.KategoriId,
				YazarId = data.YazarId,
				GoruntulenmeSayisi = data.GoruntulenmeSayisi,
				Kategoriler = GetKategorilerSelectList(),
				Yazarlar = GetYazarlarSelectList()
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult BlogGuncelle(BlogViewModel model)
		{
			var blog = new BloglarDto
			{
				Id = model.Id,
				Baslik = model.Baslik,
				Ozet = model.Ozet,
				Icerik = model.Icerik,
				GorselUrl = model.GorselUrl,
				Etiketler = model.Etiketler,
				YayinTarihi = model.YayinTarihi,
				AktifMi = model.AktifMi,
				KategoriId = model.KategoriId,
				YazarId = model.YazarId
			};
			_blogApiRequest.UpdateBlog(blog);
			return RedirectToAction("Index");
		}

		public IActionResult Sil(int blogId)
		{
			_blogApiRequest.DeleteBlog(blogId);
			return RedirectToAction("Index");
		}

		private List<SelectListItem> GetKategorilerSelectList()
		{
			var kategoriler = _blogKategoriApiRequest.GetAktifKategoriler();
			if (kategoriler == null) return new List<SelectListItem>();

			return kategoriler.Select(k => new SelectListItem
			{
				Value = k.Id.ToString(),
				Text = k.Adi
			}).ToList();
		}

		private List<SelectListItem> GetYazarlarSelectList()
		{
			var kullanicilar = _kullaniciApiRequest.GetAllKullanici();
			if (kullanicilar == null) return new List<SelectListItem>();

			return kullanicilar.Select(k => new SelectListItem
			{
				Value = k.Id.ToString(),
				Text = $"{k.Ad} {k.Soyad}"
			}).ToList();
		}
	}
}
