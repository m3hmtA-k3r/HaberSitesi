using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminUI.Controllers
{
	[Authorize]
	public class HomeController(IHaberApiRequest haberApiRequest, IYorumApiRequest yorumApiRequest) : Controller
	{
		private readonly IHaberApiRequest _haberApiRequest = haberApiRequest;
		private readonly IYorumApiRequest _yorumApiRequest = yorumApiRequest;

        public IActionResult Index()
		{
            var haberler = _haberApiRequest.GetAllHaber().Count;
            var bekleyenYorumSayisi = _yorumApiRequest.GetOnayBekleyenYorumSayisi();
            ViewBag.BekleyenYorum = bekleyenYorumSayisi;
            ViewBag.Haberler = haberler;

            return View();
		}

	}
}
