using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace AdminUI.Controllers
{
	[Authorize]
	public class BlogKategoriController : Controller
	{
		private readonly IBlogKategoriApiRequest _blogKategoriApiRequest;

		public BlogKategoriController(IBlogKategoriApiRequest blogKategoriApiRequest)
		{
			_blogKategoriApiRequest = blogKategoriApiRequest;
		}

		public IActionResult Index()
		{
			var pageData = _blogKategoriApiRequest.GetKategoriler();
			return View(pageData);
		}

		public IActionResult Ekle()
		{
			var model = new BlogKategoriViewModel
			{
				AktifMi = true,
				Sira = 0
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult KategoriEkle(BlogKategoriViewModel model)
		{
			var kategori = new BlogKategorilerDto
			{
				Adi = model.Adi,
				Aciklama = model.Aciklama,
				Sira = model.Sira,
				AktifMi = model.AktifMi
			};
			_blogKategoriApiRequest.KategoriEkle(kategori);
			return RedirectToAction("Index");
		}

		public IActionResult Guncelle(int kategoriId)
		{
			var data = _blogKategoriApiRequest.GetKategoriById(kategoriId);
			if (data == null)
				return RedirectToAction("Index");

			var model = new BlogKategoriViewModel
			{
				Id = data.Id,
				Adi = data.Adi,
				Aciklama = data.Aciklama,
				Sira = data.Sira,
				AktifMi = data.AktifMi
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult KategoriGuncelle(BlogKategoriViewModel model)
		{
			var kategori = new BlogKategorilerDto
			{
				Id = model.Id.Value,
				Adi = model.Adi,
				Aciklama = model.Aciklama,
				Sira = model.Sira,
				AktifMi = model.AktifMi
			};
			_blogKategoriApiRequest.UpdateKategori(kategori);
			return RedirectToAction("Index");
		}

		public IActionResult Sil(int kategoriId)
		{
			_blogKategoriApiRequest.DeleteKategori(kategoriId);
			return RedirectToAction("Index");
		}
	}
}
