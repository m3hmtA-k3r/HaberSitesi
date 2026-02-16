using Newtonsoft.Json;

namespace Shared.Dtos
{
    public class RolDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("rolAdi")]
        public string RolAdi { get; set; } = string.Empty;

        [JsonProperty("aciklama")]
        public string? Aciklama { get; set; }

        [JsonProperty("aktifMi")]
        public bool AktifMi { get; set; }
    }

    public class KullaniciRolDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("kullaniciId")]
        public int KullaniciId { get; set; }

        [JsonProperty("rolId")]
        public int RolId { get; set; }

        [JsonProperty("rolAdi")]
        public string? RolAdi { get; set; }

        [JsonProperty("atanmaTarihi")]
        public DateTime AtanmaTarihi { get; set; }
    }
}
