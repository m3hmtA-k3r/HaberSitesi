using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogApiRequest _blogApiRequest;
        private readonly IBlogKategoriApiRequest _blogKategoriApiRequest;
        private readonly IBlogYorumApiRequest _blogYorumApiRequest;

        public BlogController(
            IBlogApiRequest blogApiRequest,
            IBlogKategoriApiRequest blogKategoriApiRequest,
            IBlogYorumApiRequest blogYorumApiRequest)
        {
            _blogApiRequest = blogApiRequest;
            _blogKategoriApiRequest = blogKategoriApiRequest;
            _blogYorumApiRequest = blogYorumApiRequest;
        }

        [HttpGet]
        public IActionResult Index(int? kategoriId = null, int sayfa = 1)
        {
            const int sayfaBoyutu = 9;

            var bloglar = _blogApiRequest.GetAktifBloglar();
            var kategoriler = _blogKategoriApiRequest.GetAktifKategoriler();

            if (bloglar == null) bloglar = new List<BloglarDto>();
            if (kategoriler == null) kategoriler = new List<BlogKategorilerDto>();

            // Kategori filtresi
            if (kategoriId.HasValue && kategoriId.Value > 0)
            {
                bloglar = bloglar.Where(x => x.KategoriId == kategoriId.Value).ToList();
            }

            // Tarihe gore sirala
            var siraliBloglar = bloglar.OrderByDescending(x => x.YayinTarihi).ToList();

            // Pagination
            var paginationInfo = PaginationHelper.GetPaginationInfo(siraliBloglar.Count, sayfa, sayfaBoyutu);
            var sayfaliBloglar = PaginationHelper.GetPaginatedList(siraliBloglar, sayfa, sayfaBoyutu);

            var model = new BlogListViewModel
            {
                Bloglar = sayfaliBloglar,
                Kategoriler = kategoriler.OrderBy(x => x.Ad).ToList(),
                SeciliKategoriId = kategoriId,
                Pagination = paginationInfo
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Detay(int id)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var blog = _blogApiRequest.GetBlogById(id);
            if (blog == null || !blog.AktifMi)
                return RedirectToAction("Index");

            // Görüntülenme sayısını artır (API tarafında yapılıyor)

            var kategoriler = _blogKategoriApiRequest.GetAktifKategoriler();
            var tumBloglar = _blogApiRequest.GetAktifBloglar();
            var tumYorumlar = _blogYorumApiRequest.GetYorumlarByBlog(id);
            var yorumlar = tumYorumlar?.Where(y => y.OnaylandiMi && y.AktifMi).ToList();

            // İlgili bloglar (aynı kategoriden)
            var ilgiliBloglar = tumBloglar?
                .Where(x => x.KategoriId == blog.KategoriId && x.Id != blog.Id)
                .OrderByDescending(x => x.YayinTarihi)
                .Take(4)
                .ToList() ?? new List<BloglarDto>();

            var model = new BlogDetayViewModel
            {
                Blog = blog,
                IlgiliBloglar = ilgiliBloglar,
                Kategoriler = kategoriler ?? new List<BlogKategorilerDto>(),
                Yorumlar = yorumlar ?? new List<BlogYorumlarDto>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YorumYap(int BlogId, string Ad, string? Email, string Yorum)
        {
            if (BlogId <= 0)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(Ad) ||
                string.IsNullOrWhiteSpace(Yorum))
            {
                TempData["Error"] = "Lutfen tum alanlari doldurun.";
                return RedirectToAction("Detay", new { id = BlogId });
            }

            var model = new BlogYorumlarDto
            {
                BlogId = BlogId,
                Ad = Ad,
                Eposta = Email ?? string.Empty,
                Yorum = Yorum,
                OnaylandiMi = false,
                AktifMi = true,
                OlusturmaTarihi = DateTime.Now
            };

            var result = _blogYorumApiRequest.YorumEkle(model);

            if (result != null)
            {
                TempData["Success"] = "Yorumunuz gonderildi ve moderasyon onayindan sonra yayinlanacaktir.";
            }
            else
            {
                TempData["Error"] = "Yorum gonderilemedi. Lutfen tekrar deneyin.";
            }

            return RedirectToAction("Detay", new { id = BlogId });
        }
    }
}
