namespace Application.DTOs
{
	public class YorumlarDto
	{
		public int Id { get; set; }
		public string Ad { get; set; } = string.Empty;
		public string Soyad { get; set; } = string.Empty;
		public string Eposta { get; set; } = string.Empty;
		public string Baslik { get; set; } = string.Empty;
		public string Icerik { get; set; } = string.Empty;
		public int HaberId { get; set; }
		public string? HaberBaslik { get; set; }
		public DateTime EklenmeTarihi { get; set; }
		public bool Aktifmi { get; set; }
	}
}
