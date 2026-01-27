using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using WebUI.Models;

namespace WebUI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISlaytApiRequest _slaytService;
        private readonly IHaberApiRequest _haberService;
        public HomeController(ISlaytApiRequest slaytService, IHaberApiRequest haberService)
        {
			_slaytService = slaytService;
			_haberService = haberService;
        }
        public IActionResult Index()
		{
			// Sadece aktif slayt ve haberleri getir
			var allSlaytlar = _slaytService.GetAllSlayt();
			var allHaberler = _haberService.GetAllHaber();

			// Debug: Log the counts
			Console.WriteLine($"DEBUG: Toplam Slayt = {allSlaytlar?.Count() ?? 0}");
			Console.WriteLine($"DEBUG: Toplam Haber = {allHaberler?.Count() ?? 0}");

			List<SlaytlarDto> slaytlar = allSlaytlar?
				.Where(x => x.Aktifmi)
				.OrderByDescending(x => x.Id)
				.ToList() ?? new List<SlaytlarDto>();

			var haberler = allHaberler?
				.Where(x => x.Aktifmi)
				.OrderByDescending(x => x.EklenmeTarihi)
				.Take(12)
				.ToList() ?? new List<HaberlerDto>();

			Console.WriteLine($"DEBUG: Aktif Slayt = {slaytlar.Count}");
			Console.WriteLine($"DEBUG: Aktif Haber = {haberler.Count}");

			AnasayfaViewModel model = new AnasayfaViewModel
			{
				Slaytlar = slaytlar,
				Haberler = haberler
			};

			return View(model);
		}
	}
}
