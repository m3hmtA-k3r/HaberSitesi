using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Shared.Dtos;

namespace Business.Base
{
    public class MenuManager : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Menu Operations

        public List<MenulerDto> GetMenuler()
        {
            var response = _unitOfWork.MenulerRepository.GetAll()
                .OrderBy(m => m.Sira)
                .ToList();
            return response.Select(item => MapToDto(item)).ToList();
        }

        public List<MenulerDto> GetAktifMenuler()
        {
            var response = _unitOfWork.MenulerRepository.GetAll()
                .Where(m => m.AktifMi)
                .OrderBy(m => m.Sira)
                .ToList();
            return response.Select(item => MapToDto(item)).ToList();
        }

        public MenulerDto GetMenuById(int id)
        {
            var response = _unitOfWork.MenulerRepository.GetById(id);
            return MapToDto(response);
        }

        public MenulerDto InsertMenu(MenulerCreateDto model)
        {
            var entity = new Menuler
            {
                Adi = model.Adi,
                Ikon = model.Ikon,
                Sira = model.Sira,
                AktifMi = model.AktifMi,
                CollapseId = model.CollapseId ?? $"menu_{Guid.NewGuid().ToString("N").Substring(0, 8)}"
            };

            var response = _unitOfWork.MenulerRepository.Insert(entity);
            _unitOfWork.SaveChanges();

            // Handle role assignments
            if (model.RolIdler != null && model.RolIdler.Any())
            {
                foreach (var rolId in model.RolIdler)
                {
                    _unitOfWork.MenuRollerRepository.Insert(new MenuRoller
                    {
                        MenuId = response.Id,
                        RolId = rolId
                    });
                }
                _unitOfWork.SaveChanges();
            }

            return MapToDto(response);
        }

        public MenulerDto UpdateMenu(MenulerUpdateDto model)
        {
            var menu = _unitOfWork.MenulerRepository.GetById(model.Id);
            if (menu == null)
                return null;

            menu.Adi = model.Adi;
            menu.Ikon = model.Ikon;
            menu.Sira = model.Sira;
            menu.AktifMi = model.AktifMi;
            menu.CollapseId = model.CollapseId;

            var response = _unitOfWork.MenulerRepository.Update(menu);
            _unitOfWork.SaveChanges();

            // Update role assignments
            if (model.RolIdler != null)
            {
                // Remove existing roles
                var existingRoles = _unitOfWork.MenuRollerRepository.GetAll()
                    .Where(mr => mr.MenuId == model.Id)
                    .ToList();
                foreach (var role in existingRoles)
                {
                    _unitOfWork.MenuRollerRepository.Delete(role);
                }

                // Add new roles
                foreach (var rolId in model.RolIdler)
                {
                    _unitOfWork.MenuRollerRepository.Insert(new MenuRoller
                    {
                        MenuId = model.Id,
                        RolId = rolId
                    });
                }
                _unitOfWork.SaveChanges();
            }

            return MapToDto(response);
        }

        public bool DeleteMenu(int id)
        {
            var menu = _unitOfWork.MenulerRepository.GetById(id);
            if (menu == null)
                return false;

            // Delete related menu items first
            var menuOgeleri = _unitOfWork.MenuOgeleriRepository.GetAll()
                .Where(mo => mo.MenuId == id)
                .ToList();
            foreach (var oge in menuOgeleri)
            {
                // Delete item role assignments
                var ogeRoller = _unitOfWork.MenuOgeRollerRepository.GetAll()
                    .Where(mor => mor.MenuOgeId == oge.Id)
                    .ToList();
                foreach (var rol in ogeRoller)
                {
                    _unitOfWork.MenuOgeRollerRepository.Delete(rol);
                }
                _unitOfWork.MenuOgeleriRepository.Delete(oge);
            }

            // Delete menu role assignments
            var menuRoller = _unitOfWork.MenuRollerRepository.GetAll()
                .Where(mr => mr.MenuId == id)
                .ToList();
            foreach (var rol in menuRoller)
            {
                _unitOfWork.MenuRollerRepository.Delete(rol);
            }

            var result = _unitOfWork.MenulerRepository.Delete(menu);
            if (result)
            {
                _unitOfWork.SaveChanges();
            }
            return result;
        }

        #endregion

        #region Menu Item Operations

        public List<MenuOgeleriDto> GetMenuOgeleri()
        {
            var response = _unitOfWork.MenuOgeleriRepository.GetAll()
                .OrderBy(mo => mo.MenuId)
                .ThenBy(mo => mo.Sira)
                .ToList();
            return response.Select(item => MapToDto(item)).ToList();
        }

        public List<MenuOgeleriDto> GetMenuOgeleriByMenuId(int menuId)
        {
            var response = _unitOfWork.MenuOgeleriRepository.GetAll()
                .Where(mo => mo.MenuId == menuId)
                .OrderBy(mo => mo.Sira)
                .ToList();
            return response.Select(item => MapToDto(item)).ToList();
        }

        public List<MenuOgeleriDto> GetBagimsizMenuOgeleri()
        {
            var response = _unitOfWork.MenuOgeleriRepository.GetAll()
                .Where(mo => mo.MenuId == null)
                .OrderBy(mo => mo.Sira)
                .ToList();
            return response.Select(item => MapToDto(item)).ToList();
        }

        public MenuOgeleriDto GetMenuOgesiById(int id)
        {
            var response = _unitOfWork.MenuOgeleriRepository.GetById(id);
            return MapToDto(response);
        }

        public MenuOgeleriDto InsertMenuOgesi(MenuOgeleriCreateDto model)
        {
            var entity = new MenuOgeleri
            {
                MenuId = model.MenuId,
                Adi = model.Adi,
                Url = model.Url,
                Ikon = model.Ikon,
                Sira = model.Sira,
                AktifMi = model.AktifMi
            };

            var response = _unitOfWork.MenuOgeleriRepository.Insert(entity);
            _unitOfWork.SaveChanges();

            // Handle role assignments
            if (model.RolIdler != null && model.RolIdler.Any())
            {
                foreach (var rolId in model.RolIdler)
                {
                    _unitOfWork.MenuOgeRollerRepository.Insert(new MenuOgeRoller
                    {
                        MenuOgeId = response.Id,
                        RolId = rolId
                    });
                }
                _unitOfWork.SaveChanges();
            }

            return MapToDto(response);
        }

        public MenuOgeleriDto UpdateMenuOgesi(MenuOgeleriUpdateDto model)
        {
            var menuOgesi = _unitOfWork.MenuOgeleriRepository.GetById(model.Id);
            if (menuOgesi == null)
                return null;

            menuOgesi.MenuId = model.MenuId;
            menuOgesi.Adi = model.Adi;
            menuOgesi.Url = model.Url;
            menuOgesi.Ikon = model.Ikon;
            menuOgesi.Sira = model.Sira;
            menuOgesi.AktifMi = model.AktifMi;

            var response = _unitOfWork.MenuOgeleriRepository.Update(menuOgesi);
            _unitOfWork.SaveChanges();

            // Update role assignments
            if (model.RolIdler != null)
            {
                // Remove existing roles
                var existingRoles = _unitOfWork.MenuOgeRollerRepository.GetAll()
                    .Where(mor => mor.MenuOgeId == model.Id)
                    .ToList();
                foreach (var role in existingRoles)
                {
                    _unitOfWork.MenuOgeRollerRepository.Delete(role);
                }

                // Add new roles
                foreach (var rolId in model.RolIdler)
                {
                    _unitOfWork.MenuOgeRollerRepository.Insert(new MenuOgeRoller
                    {
                        MenuOgeId = model.Id,
                        RolId = rolId
                    });
                }
                _unitOfWork.SaveChanges();
            }

            return MapToDto(response);
        }

        public bool DeleteMenuOgesi(int id)
        {
            var menuOgesi = _unitOfWork.MenuOgeleriRepository.GetById(id);
            if (menuOgesi == null)
                return false;

            // Delete role assignments
            var ogeRoller = _unitOfWork.MenuOgeRollerRepository.GetAll()
                .Where(mor => mor.MenuOgeId == id)
                .ToList();
            foreach (var rol in ogeRoller)
            {
                _unitOfWork.MenuOgeRollerRepository.Delete(rol);
            }

            var result = _unitOfWork.MenuOgeleriRepository.Delete(menuOgesi);
            if (result)
            {
                _unitOfWork.SaveChanges();
            }
            return result;
        }

        #endregion

        #region Role-based Menu Access

        public MenuYapisiDto GetMenuYapisiByRolId(int rolId)
        {
            return GetMenuYapisiByRolIds(new List<int> { rolId });
        }

        public MenuYapisiDto GetMenuYapisiByRolIds(List<int> rolIds)
        {
            var result = new MenuYapisiDto();

            // Get menus accessible by these roles
            var menuRoller = _unitOfWork.MenuRollerRepository.GetAll()
                .Where(mr => rolIds.Contains(mr.RolId))
                .Select(mr => mr.MenuId)
                .Distinct()
                .ToList();

            var menuler = _unitOfWork.MenulerRepository.GetAll()
                .Where(m => m.AktifMi && menuRoller.Contains(m.Id))
                .OrderBy(m => m.Sira)
                .ToList();

            // Get menu items accessible by these roles
            var menuOgeRoller = _unitOfWork.MenuOgeRollerRepository.GetAll()
                .Where(mor => rolIds.Contains(mor.RolId))
                .Select(mor => mor.MenuOgeId)
                .Distinct()
                .ToList();

            foreach (var menu in menuler)
            {
                var menuDto = MapToDto(menu);

                // Get items for this menu that user has access to
                menuDto.MenuOgeleri = _unitOfWork.MenuOgeleriRepository.GetAll()
                    .Where(mo => mo.MenuId == menu.Id && mo.AktifMi && menuOgeRoller.Contains(mo.Id))
                    .OrderBy(mo => mo.Sira)
                    .Select(mo => MapToDto(mo))
                    .ToList();

                if (menuDto.MenuOgeleri.Any())
                {
                    result.Menuler.Add(menuDto);
                }
            }

            // Get independent items accessible by these roles
            result.BagimsizOgeler = _unitOfWork.MenuOgeleriRepository.GetAll()
                .Where(mo => mo.MenuId == null && mo.AktifMi && menuOgeRoller.Contains(mo.Id))
                .OrderBy(mo => mo.Sira)
                .Select(mo => MapToDto(mo))
                .ToList();

            return result;
        }

        public MenuYapisiDto GetTumMenuYapisi()
        {
            var result = new MenuYapisiDto();

            var menuler = _unitOfWork.MenulerRepository.GetAll()
                .Where(m => m.AktifMi)
                .OrderBy(m => m.Sira)
                .ToList();

            foreach (var menu in menuler)
            {
                var menuDto = MapToDto(menu);

                // Get all active items for this menu
                menuDto.MenuOgeleri = _unitOfWork.MenuOgeleriRepository.GetAll()
                    .Where(mo => mo.MenuId == menu.Id && mo.AktifMi)
                    .OrderBy(mo => mo.Sira)
                    .Select(mo => MapToDto(mo))
                    .ToList();

                result.Menuler.Add(menuDto);
            }

            // Get independent items
            result.BagimsizOgeler = _unitOfWork.MenuOgeleriRepository.GetAll()
                .Where(mo => mo.MenuId == null && mo.AktifMi)
                .OrderBy(mo => mo.Sira)
                .Select(mo => MapToDto(mo))
                .ToList();

            return result;
        }

        #endregion

        #region Role Assignments

        public void MenuRolAtama(int menuId, List<int> rolIds)
        {
            // Remove existing roles
            var existingRoles = _unitOfWork.MenuRollerRepository.GetAll()
                .Where(mr => mr.MenuId == menuId)
                .ToList();
            foreach (var role in existingRoles)
            {
                _unitOfWork.MenuRollerRepository.Delete(role);
            }

            // Add new roles
            foreach (var rolId in rolIds)
            {
                _unitOfWork.MenuRollerRepository.Insert(new MenuRoller
                {
                    MenuId = menuId,
                    RolId = rolId
                });
            }
            _unitOfWork.SaveChanges();
        }

        public void MenuOgesiRolAtama(int menuOgeId, List<int> rolIds)
        {
            // Remove existing roles
            var existingRoles = _unitOfWork.MenuOgeRollerRepository.GetAll()
                .Where(mor => mor.MenuOgeId == menuOgeId)
                .ToList();
            foreach (var role in existingRoles)
            {
                _unitOfWork.MenuOgeRollerRepository.Delete(role);
            }

            // Add new roles
            foreach (var rolId in rolIds)
            {
                _unitOfWork.MenuOgeRollerRepository.Insert(new MenuOgeRoller
                {
                    MenuOgeId = menuOgeId,
                    RolId = rolId
                });
            }
            _unitOfWork.SaveChanges();
        }

        #endregion

        #region Mapping Methods

        private MenulerDto MapToDto(Menuler entity)
        {
            if (entity == null) return null;

            var rolIdler = _unitOfWork.MenuRollerRepository.GetAll()
                .Where(mr => mr.MenuId == entity.Id)
                .Select(mr => mr.RolId)
                .ToList();

            return new MenulerDto
            {
                Id = entity.Id,
                Adi = entity.Adi,
                Ikon = entity.Ikon,
                Sira = entity.Sira,
                AktifMi = entity.AktifMi,
                CollapseId = entity.CollapseId,
                RolIdler = rolIdler
            };
        }

        private MenuOgeleriDto MapToDto(MenuOgeleri entity)
        {
            if (entity == null) return null;

            var menu = entity.MenuId.HasValue
                ? _unitOfWork.MenulerRepository.GetById(entity.MenuId.Value)
                : null;

            var rolIdler = _unitOfWork.MenuOgeRollerRepository.GetAll()
                .Where(mor => mor.MenuOgeId == entity.Id)
                .Select(mor => mor.RolId)
                .ToList();

            return new MenuOgeleriDto
            {
                Id = entity.Id,
                MenuId = entity.MenuId,
                MenuAdi = menu?.Adi,
                Adi = entity.Adi,
                Url = entity.Url,
                Ikon = entity.Ikon,
                Sira = entity.Sira,
                AktifMi = entity.AktifMi,
                RolIdler = rolIdler
            };
        }

        #endregion
    }
}
