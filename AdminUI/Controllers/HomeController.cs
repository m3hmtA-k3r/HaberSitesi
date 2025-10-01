using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminUI.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IHaberApiRequest _haberApiRequest;
        public HomeController(IHaberApiRequest haberApiRequest)
        {
			_haberApiRequest = haberApiRequest;
        }
        public IActionResult Index()
		{
            //var haberler = _haberApiRequest.GetAllHaber();

            return View();
		}
	}
}
