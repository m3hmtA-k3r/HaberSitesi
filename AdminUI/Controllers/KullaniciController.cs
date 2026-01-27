using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Dtos;

namespace AdminUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KullaniciController : Controller
    {
        private readonly IKullaniciApiRequest _kullaniciApiRequest;
        private readonly IRolApiRequest _rolApiRequest;
        private readonly ICommonApiRequest _commonApiRequest;

        public KullaniciController(
            IKullaniciApiRequest kullaniciApiRequest,
            IRolApiRequest rolApiRequest,
            ICommonApiRequest commonApiRequest)
        {
            _kullaniciApiRequest = kullaniciApiRequest;
            _rolApiRequest = rolApiRequest;
            _commonApiRequest = commonApiRequest;
        }

        public IActionResult Index()
        {
            var kullanicilar = _kullaniciApiRequest.GetAllKullanici();

            if (kullanicilar == null)
            {
                TempData["Error"] = "Kullanıcı listesi alınamadı. API bağlantısını kontrol edin.";
                return View(new List<KullaniciListViewModel>());
            }

            var model = kullanicilar.Select(k => new KullaniciListViewModel
            {
                Id = k.Id,
                AdSoyad = $"{k.Ad} {k.Soyad}",
                Eposta = k.Eposta,
                AktifMi = k.AktifMi,
                OlusturmaTarihi = k.OlusturmaTarihi,
                SonGirisTarihi = k.SonGirisTarihi,
                Roller = k.Roller
            }).ToList();

            return View(model);
        }

        public IActionResult Ekle()
        {
            var roller = _rolApiRequest.GetAllRol()
                .Select(r => new SelectListItem { Text = r.RolAdi, Value = r.Id.ToString() })
                .ToList();

            var model = new KullaniciViewModel
            {
                TumRoller = roller
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult KullaniciEkle(KullaniciViewModel model)
        {
            if (model.Sifre != model.SifreTekrar)
            {
                ModelState.AddModelError("SifreTekrar", "Sifreler eslesmiyor");
                model.TumRoller = _rolApiRequest.GetAllRol()
                    .Select(r => new SelectListItem { Text = r.RolAdi, Value = r.Id.ToString() })
                    .ToList();
                return View("Ekle", model);
            }

            string resimUrl = model.Resim ?? "";
            if (model.ResimFile != null)
            {
                resimUrl = _commonApiRequest.Upload(model.ResimFile) ?? "";
            }

            var createDto = new KullaniciCreateDto
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Eposta = model.Eposta,
                Sifre = model.Sifre ?? "",
                Resim = resimUrl,
                AktifMi = model.AktifMi,
                RolIdler = model.SeciliRoller
            };

            _kullaniciApiRequest.CreateKullanici(createDto);

            return RedirectToAction("Index");
        }

        public IActionResult Guncelle(int kullaniciId)
        {
            var kullanici = _kullaniciApiRequest.GetKullaniciById(kullaniciId);
            if (kullanici == null)
            {
                return NotFound();
            }

            var roller = _rolApiRequest.GetAllRol()
                .Select(r => new SelectListItem
                {
                    Text = r.RolAdi,
                    Value = r.Id.ToString(),
                    Selected = kullanici.Roller.Contains(r.RolAdi)
                })
                .ToList();

            var model = new KullaniciViewModel
            {
                Id = kullanici.Id,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Eposta = kullanici.Eposta,
                Resim = kullanici.Resim,
                AktifMi = kullanici.AktifMi,
                OlusturmaTarihi = kullanici.OlusturmaTarihi,
                SonGirisTarihi = kullanici.SonGirisTarihi,
                TumRoller = roller,
                MevcutRoller = kullanici.Roller
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult KullaniciGuncelle(KullaniciViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Sifre) && model.Sifre != model.SifreTekrar)
            {
                ModelState.AddModelError("SifreTekrar", "Sifreler eslesmiyor");
                model.TumRoller = _rolApiRequest.GetAllRol()
                    .Select(r => new SelectListItem { Text = r.RolAdi, Value = r.Id.ToString() })
                    .ToList();
                return View("Guncelle", model);
            }

            string resimUrl = model.Resim ?? "";
            if (model.ResimFile != null)
            {
                var uploadedResim = _commonApiRequest.Upload(model.ResimFile);
                if (!string.IsNullOrEmpty(uploadedResim))
                {
                    resimUrl = uploadedResim;
                }
            }

            var updateDto = new KullaniciUpdateDto
            {
                Id = model.Id,
                Ad = model.Ad,
                Soyad = model.Soyad,
                Eposta = model.Eposta,
                Sifre = model.Sifre,
                Resim = resimUrl,
                AktifMi = model.AktifMi
            };

            _kullaniciApiRequest.UpdateKullanici(updateDto);

            return RedirectToAction("Index");
        }

        public IActionResult Sil(int kullaniciId)
        {
            _kullaniciApiRequest.DeleteKullanici(kullaniciId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SilAjax(int kullaniciId)
        {
            try
            {
                _kullaniciApiRequest.DeleteKullanici(kullaniciId);
                return Json(new { success = true, message = "Kullanici silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult RollerDuzenle(int kullaniciId)
        {
            var kullanici = _kullaniciApiRequest.GetKullaniciById(kullaniciId);
            if (kullanici == null)
            {
                return NotFound();
            }

            var tumRoller = _rolApiRequest.GetAllRol();
            var kullaniciRolleri = _kullaniciApiRequest.GetKullaniciRolleri(kullaniciId);
            var kullaniciRolIdleri = kullaniciRolleri.Select(r => r.Id).ToList();

            var model = new KullaniciViewModel
            {
                Id = kullanici.Id,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                TumRoller = tumRoller.Select(r => new SelectListItem
                {
                    Text = r.RolAdi,
                    Value = r.Id.ToString(),
                    Selected = kullaniciRolIdleri.Contains(r.Id)
                }).ToList(),
                SeciliRoller = kullaniciRolIdleri
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult RollerKaydet(int kullaniciId, List<int>? seciliRoller)
        {
            var mevcutRoller = _kullaniciApiRequest.GetKullaniciRolleri(kullaniciId);
            var mevcutRolIdleri = mevcutRoller.Select(r => r.Id).ToList();
            var yeniRolIdleri = seciliRoller ?? new List<int>();

            // Remove roles that are no longer selected
            foreach (var rolId in mevcutRolIdleri)
            {
                if (!yeniRolIdleri.Contains(rolId))
                {
                    _kullaniciApiRequest.KaldirRol(kullaniciId, rolId);
                }
            }

            // Add newly selected roles
            foreach (var rolId in yeniRolIdleri)
            {
                if (!mevcutRolIdleri.Contains(rolId))
                {
                    _kullaniciApiRequest.AtaRol(kullaniciId, rolId);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
