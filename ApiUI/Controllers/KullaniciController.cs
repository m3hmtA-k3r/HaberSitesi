using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class KullaniciController : ControllerBase
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IRolService _rolService;

        public KullaniciController(IKullaniciService kullaniciService, IRolService rolService)
        {
            _kullaniciService = kullaniciService;
            _rolService = rolService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var kullanicilar = _kullaniciService.GetKullanicilar();
            return Ok(kullanicilar);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var kullanici = _kullaniciService.GetKullaniciById(id);
            if (kullanici == null)
            {
                return NotFound();
            }
            return Ok(kullanici);
        }

        [HttpPost]
        [AllowAnonymous] // İlk kullanıcı oluşturma için gerekli
        public IActionResult Create([FromBody] KullaniciCreateDto model)
        {
            // Check if email already exists
            var existing = _kullaniciService.GetKullaniciByEposta(model.Eposta);
            if (existing != null)
            {
                return BadRequest(new { message = "Bu e-posta adresi zaten kayitli" });
            }

            var kullanici = _kullaniciService.CreateKullanici(model);
            return CreatedAtAction(nameof(GetById), new { id = kullanici.Id }, kullanici);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] KullaniciUpdateDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var kullanici = _kullaniciService.UpdateKullanici(model);
            if (kullanici == null)
            {
                return NotFound();
            }

            return Ok(kullanici);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _kullaniciService.DeleteKullanici(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{kullaniciId}/rol/{rolId}")]
        public IActionResult AtaRol(int kullaniciId, int rolId)
        {
            var result = _rolService.AtaRol(kullaniciId, rolId);
            if (!result)
            {
                return BadRequest(new { message = "Rol atama basarisiz" });
            }

            return Ok(new { message = "Rol basariyla atandi" });
        }

        [HttpDelete("{kullaniciId}/rol/{rolId}")]
        public IActionResult KaldirRol(int kullaniciId, int rolId)
        {
            var result = _rolService.KaldirRol(kullaniciId, rolId);
            if (!result)
            {
                return BadRequest(new { message = "Rol kaldirma basarisiz" });
            }

            return Ok(new { message = "Rol basariyla kaldirildi" });
        }

        [HttpGet("{kullaniciId}/roller")]
        public IActionResult GetKullaniciRolleri(int kullaniciId)
        {
            var roller = _rolService.GetKullaniciRolleri(kullaniciId);
            return Ok(roller);
        }
    }
}
