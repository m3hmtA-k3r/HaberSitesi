using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogApiRequest _blogApiRequest;
        private readonly IBlogKategoriApiRequest _blogKategoriApiRequest;
        private readonly IBlogYorumApiRequest _blogYorumApiRequest;
        private readonly IMemoryCache _cache;
        private readonly ILogger<BlogController> _logger;

        public BlogController(
            IBlogApiRequest blogApiRequest,
            IBlogKategoriApiRequest blogKategoriApiRequest,
            IBlogYorumApiRequest blogYorumApiRequest,
            IMemoryCache cache,
            ILogger<BlogController> logger)
        {
            _blogApiRequest = blogApiRequest;
            _blogKategoriApiRequest = blogKategoriApiRequest;
            _blogYorumApiRequest = blogYorumApiRequest;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int? kategoriId = null, int sayfa = 1)
        {
            try
            {
                const int sayfaBoyutu = 9;

                var bloglar = _blogApiRequest.GetAktifBloglar() ?? new List<BloglarDto>();
                var kategoriler = _cache.GetOrCreate("blog_kategoriler", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _blogKategoriApiRequest.GetAktifKategoriler() ?? new List<BlogKategorilerDto>();
                })!;

                if (kategoriId.HasValue && kategoriId.Value > 0)
                    bloglar = bloglar.Where(x => x.KategoriId == kategoriId.Value).ToList();

                var siraliBloglar = bloglar.OrderByDescending(x => x.YayinTarihi).ToList();

                var paginationInfo = PaginationHelper.GetPaginationInfo(siraliBloglar.Count, sayfa, sayfaBoyutu);
                var sayfaliBloglar = PaginationHelper.GetPaginatedList(siraliBloglar, sayfa, sayfaBoyutu);

                var model = new BlogListViewModel
                {
                    Bloglar = sayfaliBloglar,
                    Kategoriler = kategoriler.OrderBy(x => x.Adi).ToList(),
                    SeciliKategoriId = kategoriId,
                    Pagination = paginationInfo
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Blog listesi yuklenirken hata olustu");
                return View(new BlogListViewModel());
            }
        }

        [HttpGet]
        public IActionResult Detay(int id)
        {
            try
            {
                if (id <= 0)
                    return RedirectToAction("Index");

                var blog = _blogApiRequest.GetBlogById(id);
                if (blog == null || !blog.AktifMi)
                    return RedirectToAction("Index");

                var kategoriler = _cache.GetOrCreate("blog_kategoriler", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _blogKategoriApiRequest.GetAktifKategoriler();
                });
                var tumBloglar = _blogApiRequest.GetAktifBloglar();
                var tumYorumlar = _blogYorumApiRequest.GetYorumlarByBlog(id);
                var yorumlar = tumYorumlar?.Where(y => y.OnaylandiMi && y.AktifMi).ToList();

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Blog detayi yuklenirken hata olustu: {BlogId}", id);
                return RedirectToAction("Index");
            }
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

            try
            {
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
                    TempData["Success"] = "Yorumunuz gonderildi ve moderasyon onayindan sonra yayinlanacaktir.";
                else
                    TempData["Error"] = "Yorum gonderilemedi. Lutfen tekrar deneyin.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Blog yorumu gonderilemedi: Blog {BlogId}", BlogId);
                TempData["Error"] = "Yorum gonderilirken bir hata olustu. Lutfen tekrar deneyin.";
            }

            return RedirectToAction("Detay", new { id = BlogId });
        }
    }
}
