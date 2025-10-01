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
			List<SlaytlarDto> slaytlar = _slaytService.GetAllSlayt().OrderByDescending(x=>x.Id).ToList();
			var haberler = _haberService.GetAllHaber().OrderByDescending(x=>x.Id).ToList();

			AnasayfaViewModel model = new AnasayfaViewModel();
			model.Slaytlar = slaytlar;
			model.Haberler = haberler;

			return View(model);
		}
	}
}
