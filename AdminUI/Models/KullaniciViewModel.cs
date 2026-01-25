using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminUI.Models
{
    public class KullaniciViewModel
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public string? Sifre { get; set; }
        public string? SifreTekrar { get; set; }
        public string? Resim { get; set; }
        public IFormFile? ResimFile { get; set; }
        public bool AktifMi { get; set; } = true;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public List<int>? SeciliRoller { get; set; }
        public List<SelectListItem>? TumRoller { get; set; }
        public List<string>? MevcutRoller { get; set; }
    }

    public class KullaniciListViewModel
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public bool AktifMi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public List<string> Roller { get; set; } = new();
    }
}
