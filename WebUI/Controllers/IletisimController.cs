using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class IletisimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Gonder(string ad, string email, string konu, string mesaj)
        {
            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(konu) || string.IsNullOrWhiteSpace(mesaj))
            {
                TempData["Error"] = "Lutfen tum alanlari doldurun.";
                return RedirectToAction("Index");
            }

            // TODO: E-posta gonderme veya veritabanina kaydetme islemi
            // Simdilik sadece basarili mesaji gosteriyoruz

            TempData["Success"] = "Mesajiniz basariyla gonderildi. En kisa surede size donecegiz.";
            return RedirectToAction("Index");
        }
    }
}
