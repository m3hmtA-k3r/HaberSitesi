namespace Shared.Dtos
{
    public class KullaniciDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public string? Resim { get; set; }
        public bool AktifMi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public List<string> Roller { get; set; } = new();
    }

    public class KullaniciCreateDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
        public string? Resim { get; set; }
        public bool AktifMi { get; set; } = true;
        public List<int>? RolIdler { get; set; }
    }

    public class KullaniciUpdateDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public string? Sifre { get; set; }
        public string? Resim { get; set; }
        public bool AktifMi { get; set; }
    }

    public class SifreDegistirDto
    {
        public string EskiSifre { get; set; } = string.Empty;
        public string YeniSifre { get; set; } = string.Empty;
        public string YeniSifreTekrar { get; set; } = string.Empty;
    }
}
