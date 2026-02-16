using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminUI.Models
{
    public class MenuViewModel
    {
        public int? Id { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string? Ikon { get; set; }
        public int Sira { get; set; } = 0;
        public bool AktifMi { get; set; } = true;
        public string? CollapseId { get; set; }
        public List<int>? RolIdler { get; set; }

        public List<SelectListItem>? Roller { get; set; }
    }

    public class MenuOgesiViewModel
    {
        public int? Id { get; set; }
        public int? MenuId { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Ikon { get; set; }
        public int Sira { get; set; } = 0;
        public bool AktifMi { get; set; } = true;
        public List<int>? RolIdler { get; set; }

        public List<SelectListItem>? Menuler { get; set; }
        public List<SelectListItem>? Roller { get; set; }
    }
}
