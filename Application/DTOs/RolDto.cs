namespace Application.DTOs
{
    public class RolDto
    {
        public int Id { get; set; }
        public string RolAdi { get; set; } = string.Empty;
        public string? Aciklama { get; set; }
        public bool AktifMi { get; set; }
    }

    public class KullaniciRolDto
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public int RolId { get; set; }
        public string? RolAdi { get; set; }
        public DateTime AtanmaTarihi { get; set; }
    }
}
