using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminUI.Controllers
{
	[Authorize]
	public class SistemLogController : Controller
	{
		private readonly ISistemLogApiRequest _sistemLogApiRequest;

		public SistemLogController(ISistemLogApiRequest sistemLogApiRequest)
		{
			_sistemLogApiRequest = sistemLogApiRequest;
		}

		public IActionResult Index()
		{
			var loglar = _sistemLogApiRequest.GetAllLog();
			return View(loglar);
		}

		public IActionResult Sil(int logId)
		{
			_sistemLogApiRequest.DeleteLog(logId);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult SilAjax(int logId)
		{
			try
			{
				_sistemLogApiRequest.DeleteLog(logId);
				return Json(new { success = true, message = "Log silindi." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public IActionResult TumunuTemizle()
		{
			_sistemLogApiRequest.DeleteAllLogs();
			return RedirectToAction("Index");
		}
	}
}
