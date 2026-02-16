using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace ApiUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        #region Menu Endpoints

        [HttpGet]
        [Route("GetAllMenu")]
        public List<MenulerDto> GetAllMenu() => _menuService.GetMenuler();

        [HttpGet]
        [Route("GetAktifMenuler")]
        public List<MenulerDto> GetAktifMenuler() => _menuService.GetAktifMenuler();

        [HttpGet]
        [Route("GetMenuById")]
        public MenulerDto GetMenuById(int menuId) => _menuService.GetMenuById(menuId);

        [HttpPost]
        [Route("InsertMenu")]
        public MenulerDto InsertMenu(MenulerCreateDto model) => _menuService.InsertMenu(model);

        [HttpPut]
        [Route("UpdateMenu")]
        public MenulerDto UpdateMenu(MenulerUpdateDto model) => _menuService.UpdateMenu(model);

        [HttpDelete]
        [Route("DeleteMenu/{menuId}")]
        public IActionResult DeleteMenu(int menuId)
        {
            var result = _menuService.DeleteMenu(menuId);
            if (!result)
                return NotFound(new { message = "Menü bulunamadı" });
            return Ok(new { message = "Menü silindi" });
        }

        #endregion

        #region Menu Item Endpoints

        [HttpGet]
        [Route("GetAllMenuOgeleri")]
        public List<MenuOgeleriDto> GetAllMenuOgeleri() => _menuService.GetMenuOgeleri();

        [HttpGet]
        [Route("GetMenuOgeleriByMenuId")]
        public List<MenuOgeleriDto> GetMenuOgeleriByMenuId(int menuId) => _menuService.GetMenuOgeleriByMenuId(menuId);

        [HttpGet]
        [Route("GetBagimsizMenuOgeleri")]
        public List<MenuOgeleriDto> GetBagimsizMenuOgeleri() => _menuService.GetBagimsizMenuOgeleri();

        [HttpGet]
        [Route("GetMenuOgesiById")]
        public MenuOgeleriDto GetMenuOgesiById(int menuOgeId) => _menuService.GetMenuOgesiById(menuOgeId);

        [HttpPost]
        [Route("InsertMenuOgesi")]
        public MenuOgeleriDto InsertMenuOgesi(MenuOgeleriCreateDto model) => _menuService.InsertMenuOgesi(model);

        [HttpPut]
        [Route("UpdateMenuOgesi")]
        public MenuOgeleriDto UpdateMenuOgesi(MenuOgeleriUpdateDto model) => _menuService.UpdateMenuOgesi(model);

        [HttpDelete]
        [Route("DeleteMenuOgesi/{menuOgeId}")]
        public IActionResult DeleteMenuOgesi(int menuOgeId)
        {
            var result = _menuService.DeleteMenuOgesi(menuOgeId);
            if (!result)
                return NotFound(new { message = "Menü öğesi bulunamadı" });
            return Ok(new { message = "Menü öğesi silindi" });
        }

        #endregion

        #region Menu Structure Endpoints

        [HttpGet]
        [Route("GetMenuYapisiByRolId")]
        public MenuYapisiDto GetMenuYapisiByRolId(int rolId) => _menuService.GetMenuYapisiByRolId(rolId);

        [HttpPost]
        [Route("GetMenuYapisiByRolIds")]
        public MenuYapisiDto GetMenuYapisiByRolIds([FromBody] List<int> rolIds) => _menuService.GetMenuYapisiByRolIds(rolIds);

        [HttpGet]
        [Route("GetTumMenuYapisi")]
        public MenuYapisiDto GetTumMenuYapisi() => _menuService.GetTumMenuYapisi();

        #endregion

        #region Role Assignment Endpoints

        [HttpPost]
        [Route("MenuRolAtama")]
        public IActionResult MenuRolAtama(int menuId, [FromBody] List<int> rolIds)
        {
            _menuService.MenuRolAtama(menuId, rolIds);
            return Ok(new { message = "Menü rolleri güncellendi" });
        }

        [HttpPost]
        [Route("MenuOgesiRolAtama")]
        public IActionResult MenuOgesiRolAtama(int menuOgeId, [FromBody] List<int> rolIds)
        {
            _menuService.MenuOgesiRolAtama(menuOgeId, rolIds);
            return Ok(new { message = "Menü öğesi rolleri güncellendi" });
        }

        #endregion
    }
}
