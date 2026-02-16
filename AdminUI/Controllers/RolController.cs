using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminUI.Controllers
{
	[Authorize]
	public class RolController : Controller
	{
		private readonly IRolApiRequest _rolApiRequest;

		public RolController(IRolApiRequest rolApiRequest)
		{
			_rolApiRequest = rolApiRequest;
		}

		public IActionResult Index()
		{
			var roller = _rolApiRequest.GetAllRol();
			return View(roller);
		}
	}
}
