namespace Shared.Dtos
{
    /// <summary>
    /// DTO for menu groups
    /// </summary>
    public class MenulerDto
    {
        public int Id { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string? Ikon { get; set; }
        public int Sira { get; set; }
        public bool AktifMi { get; set; }
        public string? CollapseId { get; set; }
        public List<MenuOgeleriDto>? MenuOgeleri { get; set; }
        public List<int>? RolIdler { get; set; }
    }

    /// <summary>
    /// DTO for menu items
    /// </summary>
    public class MenuOgeleriDto
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public string? MenuAdi { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Ikon { get; set; }
        public int Sira { get; set; }
        public bool AktifMi { get; set; }
        public List<int>? RolIdler { get; set; }
    }

    /// <summary>
    /// DTO for creating a new menu
    /// </summary>
    public class MenulerCreateDto
    {
        public string Adi { get; set; } = string.Empty;
        public string? Ikon { get; set; }
        public int Sira { get; set; } = 0;
        public bool AktifMi { get; set; } = true;
        public string? CollapseId { get; set; }
        public List<int>? RolIdler { get; set; }
    }

    /// <summary>
    /// DTO for updating a menu
    /// </summary>
    public class MenulerUpdateDto
    {
        public int Id { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string? Ikon { get; set; }
        public int Sira { get; set; }
        public bool AktifMi { get; set; }
        public string? CollapseId { get; set; }
        public List<int>? RolIdler { get; set; }
    }

    /// <summary>
    /// DTO for creating a new menu item
    /// </summary>
    public class MenuOgeleriCreateDto
    {
        public int? MenuId { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Ikon { get; set; }
        public int Sira { get; set; } = 0;
        public bool AktifMi { get; set; } = true;
        public List<int>? RolIdler { get; set; }
    }

    /// <summary>
    /// DTO for updating a menu item
    /// </summary>
    public class MenuOgeleriUpdateDto
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Ikon { get; set; }
        public int Sira { get; set; }
        public bool AktifMi { get; set; }
        public List<int>? RolIdler { get; set; }
    }

    /// <summary>
    /// DTO for hierarchical menu structure (used in sidebar)
    /// </summary>
    public class MenuYapisiDto
    {
        public List<MenulerDto> Menuler { get; set; } = new();
        public List<MenuOgeleriDto> BagimsizOgeler { get; set; } = new();
    }
}
