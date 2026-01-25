namespace Application.DTOs
{
    public class ProfilDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public string? Resim { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public List<string> Roller { get; set; } = new();
    }

    public class ProfilGuncelleDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string? Resim { get; set; }
    }
}
