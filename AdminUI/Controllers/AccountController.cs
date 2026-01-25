using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Security.Claims;

namespace AdminUI.Controllers
{
	public class AccountController : Controller
	{
		private readonly IYazarApiRequest _yazarApiRequest;
		private readonly IAuthApiRequest _authApiRequest;
		private readonly ICommonApiRequest _commonApiRequest;

		public AccountController(
			IYazarApiRequest yazarApiRequest,
			IAuthApiRequest authApiRequest,
			ICommonApiRequest commonApiRequest)
		{
			_yazarApiRequest = yazarApiRequest;
			_authApiRequest = authApiRequest;
			_commonApiRequest = commonApiRequest;
		}

		public IActionResult Login()
		{
			return View();
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> GirisYap(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Try new auth system first
				var loginResult = _authApiRequest.Login(model.Email, model.Password);
				if (loginResult != null && loginResult.Success && loginResult.Kullanici != null)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, loginResult.Kullanici.Id.ToString()),
						new Claim(ClaimTypes.Email, model.Email),
						new Claim(ClaimTypes.Name, $"{loginResult.Kullanici.Ad} {loginResult.Kullanici.Soyad}")
					};

					// Add role claims
					if (loginResult.Roller != null)
					{
						foreach (var rol in loginResult.Roller)
						{
							claims.Add(new Claim(ClaimTypes.Role, rol));
						}
					}

					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					ClaimsPrincipal principal = new ClaimsPrincipal(identity);
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					// Store token in session
					if (!string.IsNullOrEmpty(loginResult.Token))
					{
						HttpContext.Session.SetString("JwtToken", loginResult.Token);
					}

					return Redirect("/Home/Index");
				}

				// Fallback to old Yazar system for backward compatibility
				var yazar = _yazarApiRequest.GetYazarByEmailPassword(model.Email, model.Password);
				if (yazar != null)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Email, model.Email),
						new Claim(ClaimTypes.Name, $"{yazar.Ad} {yazar.Soyad}")
					};
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					ClaimsPrincipal principal = new ClaimsPrincipal(identity);
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
					return Redirect("/Home/Index");
				}

				ModelState.AddModelError("ozelHataMesaji", "Kullanici adi veya sifre hatali!");
				return View("Login");
			}

			return View("Login");
		}

		public async Task<IActionResult> CikisYap()
		{
			HttpContext.Session.Remove("JwtToken");
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return View("Login");
		}

		[Authorize]
		public IActionResult Profil()
		{
			// Try new auth system first (JWT based)
			var profil = _authApiRequest.GetProfil();
			if (profil != null)
			{
				var model = new ProfilViewModel
				{
					Id = profil.Id,
					Ad = profil.Ad,
					Soyad = profil.Soyad,
					Eposta = profil.Eposta,
					Resim = profil.Resim,
					OlusturmaTarihi = profil.OlusturmaTarihi,
					SonGirisTarihi = profil.SonGirisTarihi,
					Roller = profil.Roller
				};
				return View(model);
			}

			// Fallback for Yazar users (no JWT token)
			var email = User.FindFirst(ClaimTypes.Email)?.Value;
			var name = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
			var nameParts = name.Split(' ', 2);

			var yazarModel = new ProfilViewModel
			{
				Id = 0,
				Ad = nameParts.Length > 0 ? nameParts[0] : "",
				Soyad = nameParts.Length > 1 ? nameParts[1] : "",
				Eposta = email ?? "",
				Resim = "",
				OlusturmaTarihi = DateTime.Now,
				SonGirisTarihi = DateTime.Now,
				Roller = new List<string> { "Yazar" }
			};

			return View(yazarModel);
		}

		[Authorize]
		[HttpPost]
		public IActionResult ProfilGuncelle(ProfilViewModel model)
		{
			string resimUrl = model.Resim ?? "";
			if (model.ResimFile != null)
			{
				var uploadedResim = _commonApiRequest.Upload(model.ResimFile);
				if (!string.IsNullOrEmpty(uploadedResim))
				{
					resimUrl = uploadedResim;
				}
			}

			var guncelleDto = new ProfilGuncelleDto
			{
				Ad = model.Ad,
				Soyad = model.Soyad,
				Resim = resimUrl
			};

			_authApiRequest.UpdateProfil(guncelleDto);

			TempData["Mesaj"] = "Profil basariyla guncellendi";
			return RedirectToAction("Profil");
		}

		[Authorize]
		public IActionResult SifreDegistir()
		{
			return View(new SifreDegistirViewModel());
		}

		[Authorize]
		[HttpPost]
		public IActionResult SifreDegistirKaydet(SifreDegistirViewModel model)
		{
			if (model.YeniSifre != model.YeniSifreTekrar)
			{
				ModelState.AddModelError("YeniSifreTekrar", "Yeni sifreler eslesmiyor");
				return View("SifreDegistir", model);
			}

			var sifreDto = new SifreDegistirDto
			{
				EskiSifre = model.EskiSifre,
				YeniSifre = model.YeniSifre,
				YeniSifreTekrar = model.YeniSifreTekrar
			};

			var result = _authApiRequest.SifreDegistir(sifreDto);
			if (!result)
			{
				ModelState.AddModelError("EskiSifre", "Eski sifre hatali");
				return View("SifreDegistir", model);
			}

			TempData["Mesaj"] = "Sifre basariyla degistirildi";
			return RedirectToAction("Profil");
		}
	}
}
