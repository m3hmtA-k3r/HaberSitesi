using Newtonsoft.Json;

namespace Shared.Dtos
{
    public class LoginResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("yazar")]
        public YazarlarDto? Yazar { get; set; }

        [JsonProperty("kullanici")]
        public KullaniciDto? Kullanici { get; set; }

        [JsonProperty("roller")]
        public List<string>? Roller { get; set; }
    }
}
