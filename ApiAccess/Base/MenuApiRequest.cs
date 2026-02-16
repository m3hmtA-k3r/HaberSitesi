using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class MenuApiRequest : IMenuApiRequest
    {
        private readonly IRequestService _requestService;

        public MenuApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }

        #region Menu Operations

        public List<MenulerDto> GetMenuler() =>
            _requestService.Get<List<MenulerDto>>("/Menu/GetAllMenu");

        public List<MenulerDto> GetAktifMenuler() =>
            _requestService.Get<List<MenulerDto>>("/Menu/GetAktifMenuler");

        public MenulerDto GetMenuById(int menuId) =>
            _requestService.Get<MenulerDto>("/Menu/GetMenuById?menuId=" + menuId);

        public MenulerDto MenuEkle(MenulerCreateDto model) =>
            _requestService.Post<MenulerDto>("/Menu/InsertMenu", model);

        public MenulerDto UpdateMenu(MenulerUpdateDto model) =>
            _requestService.Put<MenulerDto>("/Menu/UpdateMenu", model);

        public bool DeleteMenu(int menuId) =>
            _requestService.Delete<bool>("/Menu/DeleteMenu/" + menuId);

        #endregion

        #region Menu Item Operations

        public List<MenuOgeleriDto> GetMenuOgeleri() =>
            _requestService.Get<List<MenuOgeleriDto>>("/Menu/GetAllMenuOgeleri");

        public List<MenuOgeleriDto> GetMenuOgeleriByMenuId(int menuId) =>
            _requestService.Get<List<MenuOgeleriDto>>("/Menu/GetMenuOgeleriByMenuId?menuId=" + menuId);

        public List<MenuOgeleriDto> GetBagimsizMenuOgeleri() =>
            _requestService.Get<List<MenuOgeleriDto>>("/Menu/GetBagimsizMenuOgeleri");

        public MenuOgeleriDto GetMenuOgesiById(int menuOgeId) =>
            _requestService.Get<MenuOgeleriDto>("/Menu/GetMenuOgesiById?menuOgeId=" + menuOgeId);

        public MenuOgeleriDto MenuOgesiEkle(MenuOgeleriCreateDto model) =>
            _requestService.Post<MenuOgeleriDto>("/Menu/InsertMenuOgesi", model);

        public MenuOgeleriDto UpdateMenuOgesi(MenuOgeleriUpdateDto model) =>
            _requestService.Put<MenuOgeleriDto>("/Menu/UpdateMenuOgesi", model);

        public bool DeleteMenuOgesi(int menuOgeId) =>
            _requestService.Delete<bool>("/Menu/DeleteMenuOgesi/" + menuOgeId);

        #endregion

        #region Menu Structure Operations

        public MenuYapisiDto GetMenuYapisiByRolId(int rolId) =>
            _requestService.Get<MenuYapisiDto>("/Menu/GetMenuYapisiByRolId?rolId=" + rolId);

        public MenuYapisiDto GetMenuYapisiByRolIds(List<int> rolIds) =>
            _requestService.Post<MenuYapisiDto>("/Menu/GetMenuYapisiByRolIds", rolIds);

        public MenuYapisiDto GetTumMenuYapisi() =>
            _requestService.Get<MenuYapisiDto>("/Menu/GetTumMenuYapisi");

        #endregion

        #region Role Assignment Operations

        public void MenuRolAtama(int menuId, List<int> rolIds) =>
            _requestService.Post<object>("/Menu/MenuRolAtama?menuId=" + menuId, rolIds);

        public void MenuOgesiRolAtama(int menuOgeId, List<int> rolIds) =>
            _requestService.Post<object>("/Menu/MenuOgesiRolAtama?menuOgeId=" + menuOgeId, rolIds);

        #endregion
    }
}
