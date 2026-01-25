namespace AdminUI.Models
{
    public class ProfilViewModel
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public string? Resim { get; set; }
        public IFormFile? ResimFile { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public List<string> Roller { get; set; } = new();
    }

    public class SifreDegistirViewModel
    {
        public string EskiSifre { get; set; } = string.Empty;
        public string YeniSifre { get; set; } = string.Empty;
        public string YeniSifreTekrar { get; set; } = string.Empty;
    }
}
