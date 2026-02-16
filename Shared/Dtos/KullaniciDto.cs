using Newtonsoft.Json;

namespace Shared.Dtos
{
    public class KullaniciDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ad")]
        public string Ad { get; set; } = string.Empty;

        [JsonProperty("soyad")]
        public string Soyad { get; set; } = string.Empty;

        [JsonProperty("eposta")]
        public string Eposta { get; set; } = string.Empty;

        [JsonProperty("resim")]
        public string? Resim { get; set; }

        [JsonProperty("aktifMi")]
        public bool AktifMi { get; set; }

        [JsonProperty("olusturmaTarihi")]
        public DateTime OlusturmaTarihi { get; set; }

        [JsonProperty("sonGirisTarihi")]
        public DateTime? SonGirisTarihi { get; set; }

        [JsonProperty("roller")]
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
