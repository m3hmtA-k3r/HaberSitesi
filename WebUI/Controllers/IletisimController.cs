using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.Dtos;
using Shared.RateLimiting;

namespace WebUI.Controllers
{
	public class IletisimController : Controller
	{
		private readonly IIletisimApiRequest _iletisimApi;
		private readonly RateLimiter _rateLimiter;

		// Rate limit: 3 messages per 10 minutes per IP
		private const int MaxRequests = 3;
		private static readonly TimeSpan RateLimitWindow = TimeSpan.FromMinutes(10);

		public IletisimController(IIletisimApiRequest iletisimApi, IMemoryCache cache)
		{
			_iletisimApi = iletisimApi;
			_rateLimiter = new RateLimiter(cache, "contact_form", MaxRequests, RateLimitWindow);
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Gonder(string ad, string email, string konu, string mesaj)
		{
			// Rate limiting check
			var clientIp = GetClientIpAddress() ?? "unknown";
			if (!_rateLimiter.IsAllowed(clientIp))
			{
				var resetTime = _rateLimiter.GetTimeUntilReset(clientIp);
				var minutes = resetTime?.Minutes ?? 10;
				TempData["Error"] = $"Cok fazla mesaj gonderdiniz. Lutfen {minutes} dakika sonra tekrar deneyin.";
				return RedirectToAction("Index");
			}

			if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(email) ||
				string.IsNullOrWhiteSpace(konu) || string.IsNullOrWhiteSpace(mesaj))
			{
				TempData["Error"] = "Lutfen tum alanlari doldurun.";
				return RedirectToAction("Index");
			}

			// Email format validation
			if (!IsValidEmail(email))
			{
				TempData["Error"] = "Gecerli bir e-posta adresi girin.";
				return RedirectToAction("Index");
			}

			// Basic length validation
			if (ad.Length > 100 || email.Length > 255 || konu.Length > 200 || mesaj.Length > 5000)
			{
				TempData["Error"] = "Alan uzunluklari siniri asiyor.";
				return RedirectToAction("Index");
			}

			try
			{
				var model = new IletisimMesajlariDto
				{
					Ad = ad.Trim(),
					Eposta = email.Trim(),
					Konu = konu.Trim(),
					Mesaj = mesaj.Trim(),
					IpAdresi = GetClientIpAddress(),
					EklemeTarihi = DateTime.UtcNow
				};

				_iletisimApi.InsertMesaj(model);

				TempData["Success"] = "Mesajiniz basariyla gonderildi. En kisa surede size donecegiz.";
			}
			catch (Exception)
			{
				TempData["Error"] = "Mesaj gonderilirken bir hata olustu. Lutfen daha sonra tekrar deneyin.";
			}

			return RedirectToAction("Index");
		}

		private string? GetClientIpAddress()
		{
			var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
			if (!string.IsNullOrEmpty(forwardedFor))
			{
				return forwardedFor.Split(',')[0].Trim();
			}

			return HttpContext.Connection.RemoteIpAddress?.ToString();
		}

		private static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
	}
}
