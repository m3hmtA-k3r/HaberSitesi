using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using System.Security.Claims;

namespace ApiUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var (kullanici, token, hata) = _authService.Login(request.Eposta, request.Sifre);

            if (kullanici == null)
            {
                return Unauthorized(new { success = false, message = hata });
            }

            return Ok(new
            {
                success = true,
                token = token,
                kullanici = kullanici,
                roller = kullanici.Roller
            });
        }

        [HttpGet]
        [Route("profil")]
        [Authorize]
        public IActionResult GetProfil()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var profil = _authService.GetProfil(userId);
            if (profil == null)
            {
                return NotFound();
            }

            return Ok(profil);
        }

        [HttpPut]
        [Route("profil")]
        [Authorize]
        public IActionResult UpdateProfil([FromBody] ProfilGuncelleDto model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var profil = _authService.UpdateProfil(userId, model);
            if (profil == null)
            {
                return NotFound();
            }

            return Ok(profil);
        }

        [HttpPost]
        [Route("sifre-degistir")]
        [Authorize]
        public IActionResult SifreDegistir([FromBody] SifreDegistirDto model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            if (model.YeniSifre != model.YeniSifreTekrar)
            {
                return BadRequest(new { success = false, message = "Yeni sifreler eslesmiyor" });
            }

            var result = _authService.SifreDegistir(userId, model);
            if (!result)
            {
                return BadRequest(new { success = false, message = "Eski sifre hatali" });
            }

            return Ok(new { success = true, message = "Sifre basariyla degistirildi" });
        }
    }

    public class LoginRequest
    {
        public string Eposta { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }
}
