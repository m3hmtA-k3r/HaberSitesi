using Shared.Dtos;

namespace Business.Abstract
{
    public interface IMenuService
    {
        // Menu operations
        List<MenulerDto> GetMenuler();
        List<MenulerDto> GetAktifMenuler();
        MenulerDto GetMenuById(int id);
        MenulerDto InsertMenu(MenulerCreateDto model);
        MenulerDto UpdateMenu(MenulerUpdateDto model);
        bool DeleteMenu(int id);

        // Menu item operations
        List<MenuOgeleriDto> GetMenuOgeleri();
        List<MenuOgeleriDto> GetMenuOgeleriByMenuId(int menuId);
        List<MenuOgeleriDto> GetBagimsizMenuOgeleri();
        MenuOgeleriDto GetMenuOgesiById(int id);
        MenuOgeleriDto InsertMenuOgesi(MenuOgeleriCreateDto model);
        MenuOgeleriDto UpdateMenuOgesi(MenuOgeleriUpdateDto model);
        bool DeleteMenuOgesi(int id);

        // Role-based menu access
        MenuYapisiDto GetMenuYapisiByRolId(int rolId);
        MenuYapisiDto GetMenuYapisiByRolIds(List<int> rolIds);
        MenuYapisiDto GetTumMenuYapisi();

        // Role assignments
        void MenuRolAtama(int menuId, List<int> rolIds);
        void MenuOgesiRolAtama(int menuOgeId, List<int> rolIds);
    }
}
