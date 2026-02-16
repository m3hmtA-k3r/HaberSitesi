using System.Diagnostics;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISlaytApiRequest _slaytService;
        private readonly IHaberApiRequest _haberService;
		private readonly IMemoryCache _cache;
		private readonly ILogger<HomeController> _logger;

        public HomeController(ISlaytApiRequest slaytService, IHaberApiRequest haberService, IMemoryCache cache, ILogger<HomeController> logger)
        {
			_slaytService = slaytService;
			_haberService = haberService;
			_cache = cache;
			_logger = logger;
        }
        public IActionResult Index()
		{
			try
			{
				// Slaytlar - 15 dk cache (sinyal bazli invalidation ile korunuyor)
				var slaytlar = _cache.GetOrCreate("home_slaytlar", entry =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
					var all = _slaytService.GetAllSlayt();
					return all?.Where(x => x.Aktifmi).OrderByDescending(x => x.Id).ToList() ?? new List<SlaytlarDto>();
				})!;

				// Ana sayfa haberleri - 5 dk cache (sinyal bazli invalidation ile korunuyor)
				var haberler = _cache.GetOrCreate("home_haberler", entry =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
					var all = _haberService.GetAllHaber();
					return all?.Where(x => x.Aktifmi).OrderByDescending(x => x.EklenmeTarihi).Take(12).ToList() ?? new List<HaberlerDto>();
				})!;

				AnasayfaViewModel model = new AnasayfaViewModel
				{
					Slaytlar = slaytlar,
					Haberler = haberler
				};

				return View(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ana sayfa yuklenirken hata olustu");
				return View(new AnasayfaViewModel());
			}
		}

		[Route("Home/Error/{statusCode?}")]
		public IActionResult Error(int? statusCode)
		{
			var code = statusCode ?? 500;
			string title;
			string message;

			switch (code)
			{
				case 404:
					title = "Sayfa Bulunamadi";
					message = "Aradiginiz sayfa mevcut degil veya tasinmis olabilir.";
					break;
				case 403:
					title = "Erisim Engellendi";
					message = "Bu sayfaya erisim izniniz bulunmamaktadir.";
					break;
				default:
					title = "Bir Hata Olustu";
					message = "Beklenmeyen bir hata olustu. Lutfen daha sonra tekrar deneyin.";
					break;
			}

			Response.StatusCode = code;

			var model = new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = code,
				Title = title,
				Message = message
			};

			return View(model);
		}
	}
}
