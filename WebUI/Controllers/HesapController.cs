using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HesapController : Controller
    {
        private readonly IAuthApiRequest _authApiRequest;

        public HesapController(IAuthApiRequest authApiRequest)
        {
            _authApiRequest = authApiRequest;
        }

        [HttpGet]
        public IActionResult Giris()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(GirisViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginResult = _authApiRequest.Login(model.Eposta, model.Sifre);

            if (loginResult == null || !loginResult.Success || loginResult.Kullanici == null)
            {
                ModelState.AddModelError(string.Empty, loginResult?.Message ?? "E-posta veya sifre hatali");
                return View(model);
            }

            await SignInUser(loginResult);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Kayit()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Kayit(KayitViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registerResult = _authApiRequest.Register(model.Ad, model.Soyad, model.Eposta, model.Sifre);

            if (registerResult == null || !registerResult.Success || registerResult.Kullanici == null)
            {
                ModelState.AddModelError(string.Empty, registerResult?.Message ?? "Kayit sirasinda bir hata olustu");
                return View(model);
            }

            await SignInUser(registerResult);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Cikis()
        {
            HttpContext.Session.Remove("JwtToken");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(Shared.Dtos.LoginResponse loginResult)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResult.Kullanici!.Id.ToString()),
                new Claim(ClaimTypes.Email, loginResult.Kullanici.Eposta),
                new Claim(ClaimTypes.Name, $"{loginResult.Kullanici.Ad} {loginResult.Kullanici.Soyad}")
            };

            if (loginResult.Roller != null)
            {
                foreach (var rol in loginResult.Roller)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(loginResult.Token))
            {
                HttpContext.Session.SetString("JwtToken", loginResult.Token);
            }
        }
    }
}
