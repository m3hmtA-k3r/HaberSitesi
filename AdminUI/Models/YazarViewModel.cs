namespace AdminUI.Models
{
	public class YazarViewModel
	{
		public int Id { get; set; }
		public string Ad { get; set; }
		public string Soyad { get; set; }
		public string Eposta { get; set; }
		public string Sifre { get; set; }
		public string Resim { get; set; }
		public IFormFile? ResimFile { get; set; }
		public bool Aktifmi { get; set; }
	}
}
