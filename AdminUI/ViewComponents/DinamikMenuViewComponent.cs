using ApiAccess.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Security.Claims;

namespace AdminUI.ViewComponents
{
    public class DinamikMenuViewComponent : ViewComponent
    {
        private readonly IMenuApiRequest _menuApiRequest;

        public DinamikMenuViewComponent(IMenuApiRequest menuApiRequest)
        {
            _menuApiRequest = menuApiRequest;
        }

        public IViewComponentResult Invoke()
        {
            MenuYapisiDto menuYapisi;

            try
            {
                // Get user's role IDs from claims
                var roleClaims = HttpContext.User.FindAll(ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                if (roleClaims.Any())
                {
                    // Try to get role IDs from session or convert role names
                    var rolIdlerStr = HttpContext.Session.GetString("KullaniciRolIdler");
                    if (!string.IsNullOrEmpty(rolIdlerStr))
                    {
                        var rolIds = rolIdlerStr.Split(',')
                            .Select(int.Parse)
                            .ToList();
                        menuYapisi = _menuApiRequest.GetMenuYapisiByRolIds(rolIds);
                    }
                    else
                    {
                        // Fallback: Get full menu structure (for Admin or when role IDs not in session)
                        menuYapisi = _menuApiRequest.GetTumMenuYapisi();
                    }
                }
                else
                {
                    // No roles - return empty menu
                    menuYapisi = new MenuYapisiDto();
                }
            }
            catch
            {
                // On error, return empty menu structure
                menuYapisi = new MenuYapisiDto();
            }

            return View(menuYapisi);
        }
    }
}
