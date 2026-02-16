using AdminUI.Models;
using ApiAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Dtos;

namespace AdminUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MenuYonetimController : Controller
    {
        private readonly IMenuApiRequest _menuApiRequest;
        private readonly IRolApiRequest _rolApiRequest;

        public MenuYonetimController(IMenuApiRequest menuApiRequest, IRolApiRequest rolApiRequest)
        {
            _menuApiRequest = menuApiRequest;
            _rolApiRequest = rolApiRequest;
        }

        #region Menu CRUD

        public IActionResult Index()
        {
            var menuler = _menuApiRequest.GetMenuler();
            return View(menuler);
        }

        public IActionResult MenuEkle()
        {
            var model = new MenuViewModel
            {
                AktifMi = true,
                Roller = GetRollerSelectList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult MenuEkle(MenuViewModel model)
        {
            var dto = new MenulerCreateDto
            {
                Adi = model.Adi,
                Ikon = model.Ikon,
                Sira = model.Sira,
                AktifMi = model.AktifMi,
                CollapseId = model.CollapseId,
                RolIdler = model.RolIdler
            };
            _menuApiRequest.MenuEkle(dto);
            TempData["Success"] = "Menü başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        public IActionResult MenuGuncelle(int menuId)
        {
            var data = _menuApiRequest.GetMenuById(menuId);
            if (data == null)
            {
                TempData["Error"] = "Menü bulunamadı.";
                return RedirectToAction("Index");
            }

            var model = new MenuViewModel
            {
                Id = data.Id,
                Adi = data.Adi,
                Ikon = data.Ikon,
                Sira = data.Sira,
                AktifMi = data.AktifMi,
                CollapseId = data.CollapseId,
                RolIdler = data.RolIdler,
                Roller = GetRollerSelectList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult MenuGuncelle(MenuViewModel model)
        {
            var dto = new MenulerUpdateDto
            {
                Id = model.Id ?? 0,
                Adi = model.Adi,
                Ikon = model.Ikon,
                Sira = model.Sira,
                AktifMi = model.AktifMi,
                CollapseId = model.CollapseId,
                RolIdler = model.RolIdler
            };
            _menuApiRequest.UpdateMenu(dto);
            TempData["Success"] = "Menü başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MenuSil(int menuId)
        {
            try
            {
                var result = _menuApiRequest.DeleteMenu(menuId);
                if (result)
                {
                    return Json(new { success = true, message = "Menü silindi." });
                }
                return Json(new { success = false, message = "Menü silinemedi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Menu Item CRUD

        public IActionResult MenuOgeleri(int menuId)
        {
            var menu = _menuApiRequest.GetMenuById(menuId);
            if (menu == null)
            {
                TempData["Error"] = "Menü bulunamadı.";
                return RedirectToAction("Index");
            }

            var ogeler = _menuApiRequest.GetMenuOgeleriByMenuId(menuId);
            ViewBag.Menu = menu;
            return View(ogeler);
        }

        public IActionResult BagimsizOgeler()
        {
            var ogeler = _menuApiRequest.GetBagimsizMenuOgeleri();
            return View(ogeler);
        }

        public IActionResult MenuOgesiEkle(int? menuId = null)
        {
            var model = new MenuOgesiViewModel
            {
                MenuId = menuId,
                AktifMi = true,
                Menuler = GetMenulerSelectList(),
                Roller = GetRollerSelectList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult MenuOgesiEkle(MenuOgesiViewModel model)
        {
            var dto = new MenuOgeleriCreateDto
            {
                MenuId = model.MenuId,
                Adi = model.Adi,
                Url = model.Url,
                Ikon = model.Ikon,
                Sira = model.Sira,
                AktifMi = model.AktifMi,
                RolIdler = model.RolIdler
            };
            _menuApiRequest.MenuOgesiEkle(dto);
            TempData["Success"] = "Menü öğesi başarıyla eklendi.";

            if (model.MenuId.HasValue)
                return RedirectToAction("MenuOgeleri", new { menuId = model.MenuId });
            return RedirectToAction("BagimsizOgeler");
        }

        public IActionResult MenuOgesiGuncelle(int menuOgeId)
        {
            var data = _menuApiRequest.GetMenuOgesiById(menuOgeId);
            if (data == null)
            {
                TempData["Error"] = "Menü öğesi bulunamadı.";
                return RedirectToAction("Index");
            }

            var model = new MenuOgesiViewModel
            {
                Id = data.Id,
                MenuId = data.MenuId,
                Adi = data.Adi,
                Url = data.Url,
                Ikon = data.Ikon,
                Sira = data.Sira,
                AktifMi = data.AktifMi,
                RolIdler = data.RolIdler,
                Menuler = GetMenulerSelectList(),
                Roller = GetRollerSelectList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult MenuOgesiGuncelle(MenuOgesiViewModel model)
        {
            var dto = new MenuOgeleriUpdateDto
            {
                Id = model.Id ?? 0,
                MenuId = model.MenuId,
                Adi = model.Adi,
                Url = model.Url,
                Ikon = model.Ikon,
                Sira = model.Sira,
                AktifMi = model.AktifMi,
                RolIdler = model.RolIdler
            };
            _menuApiRequest.UpdateMenuOgesi(dto);
            TempData["Success"] = "Menü öğesi başarıyla güncellendi.";

            if (model.MenuId.HasValue)
                return RedirectToAction("MenuOgeleri", new { menuId = model.MenuId });
            return RedirectToAction("BagimsizOgeler");
        }

        [HttpPost]
        public IActionResult MenuOgesiSil(int menuOgeId, int? menuId = null)
        {
            try
            {
                var result = _menuApiRequest.DeleteMenuOgesi(menuOgeId);
                if (result)
                {
                    return Json(new { success = true, message = "Menü öğesi silindi." });
                }
                return Json(new { success = false, message = "Menü öğesi silinemedi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Helper Methods

        private List<SelectListItem> GetRollerSelectList()
        {
            var roller = _rolApiRequest.GetAllRol();
            if (roller == null) return new List<SelectListItem>();

            return roller.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RolAdi
            }).ToList();
        }

        private List<SelectListItem> GetMenulerSelectList()
        {
            var menuler = _menuApiRequest.GetMenuler();
            if (menuler == null) return new List<SelectListItem>();

            return menuler.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Adi
            }).ToList();
        }

        #endregion
    }
}
