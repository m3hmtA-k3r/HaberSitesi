namespace Shared.Dtos
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public YazarlarDto? Yazar { get; set; }
        public KullaniciDto? Kullanici { get; set; }
        public List<string>? Roller { get; set; }
    }
}
