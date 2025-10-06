using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminUI.Controllers
{
	[Authorize]
	public class HomeController(IHaberApiRequest haberApiRequest, IYorumApiRequest yorumApiRequest,ISlaytApiRequest slaytApiRequest) : Controller
	{
		private readonly IHaberApiRequest _haberApiRequest = haberApiRequest;
		private readonly IYorumApiRequest _yorumApiRequest = yorumApiRequest;
		private readonly ISlaytApiRequest _slaytApiRequest = slaytApiRequest;


        public IActionResult Index()
		{
            var haberler = _haberApiRequest.GetAllHaber().Count;
            var bekleyenYorumSayisi = _yorumApiRequest.GetOnayBekleyenYorumSayisi();
			var bekleyenSlaytSayisi = _slaytApiRequest.GetUnpublishedSlidesCount();

			ViewBag.BekleyenSlayt = bekleyenSlaytSayisi;
            ViewBag.BekleyenYorum = bekleyenYorumSayisi;
            ViewBag.Haberler = haberler;

            return View();
		}

	}
}
