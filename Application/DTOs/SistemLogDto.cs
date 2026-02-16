namespace Application.DTOs
{
	public class SistemLogDto
	{
		public int Id { get; set; }
		public int? KullaniciId { get; set; }
		public string? KullaniciAdi { get; set; }
		public string IslemTipi { get; set; } = string.Empty;
		public string Modul { get; set; } = string.Empty;
		public string Aciklama { get; set; } = string.Empty;
		public string? IpAdresi { get; set; }
		public DateTime Tarih { get; set; }
		public string Seviye { get; set; } = "Info";
	}
}
