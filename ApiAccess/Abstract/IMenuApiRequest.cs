using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IMenuApiRequest
    {
        // Menu operations
        List<MenulerDto> GetMenuler();
        List<MenulerDto> GetAktifMenuler();
        MenulerDto GetMenuById(int menuId);
        MenulerDto MenuEkle(MenulerCreateDto model);
        MenulerDto UpdateMenu(MenulerUpdateDto model);
        bool DeleteMenu(int menuId);

        // Menu item operations
        List<MenuOgeleriDto> GetMenuOgeleri();
        List<MenuOgeleriDto> GetMenuOgeleriByMenuId(int menuId);
        List<MenuOgeleriDto> GetBagimsizMenuOgeleri();
        MenuOgeleriDto GetMenuOgesiById(int menuOgeId);
        MenuOgeleriDto MenuOgesiEkle(MenuOgeleriCreateDto model);
        MenuOgeleriDto UpdateMenuOgesi(MenuOgeleriUpdateDto model);
        bool DeleteMenuOgesi(int menuOgeId);

        // Menu structure operations
        MenuYapisiDto GetMenuYapisiByRolId(int rolId);
        MenuYapisiDto GetMenuYapisiByRolIds(List<int> rolIds);
        MenuYapisiDto GetTumMenuYapisi();

        // Role assignment operations
        void MenuRolAtama(int menuId, List<int> rolIds);
        void MenuOgesiRolAtama(int menuOgeId, List<int> rolIds);
    }
}
