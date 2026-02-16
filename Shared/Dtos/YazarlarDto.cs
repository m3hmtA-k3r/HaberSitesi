namespace Shared.Dtos
{
	public class YazarlarDto
	{
		public int Id { get; set; }
		public string Ad { get; set; } = string.Empty;
		public string Soyad { get; set; } = string.Empty;
		public string Eposta { get; set; } = string.Empty;
		public string Sifre { get; set; } = string.Empty;
		public string Resim { get; set; } = string.Empty;
		public bool Aktifmi { get; set; }
	}
}
