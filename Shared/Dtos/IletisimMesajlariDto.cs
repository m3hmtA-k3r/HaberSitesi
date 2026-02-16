namespace Shared.Dtos
{
	public class IletisimMesajlariDto
	{
		public int Id { get; set; }
		public string Ad { get; set; } = string.Empty;
		public string Eposta { get; set; } = string.Empty;
		public string Konu { get; set; } = string.Empty;
		public string Mesaj { get; set; } = string.Empty;
		public string? IpAdresi { get; set; }
		public DateTime EklemeTarihi { get; set; }
		public bool OkunduMu { get; set; }
		public bool CevaplandiMi { get; set; }
		public DateTime? CevapTarihi { get; set; }
	}
}
