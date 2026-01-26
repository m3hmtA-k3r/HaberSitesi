namespace AdminUI.Models
{
	public class BlogKategoriViewModel
	{
		public int? Id { get; set; }
		public string Adi { get; set; } = string.Empty;
		public string? Aciklama { get; set; }
		public int Sira { get; set; }
		public bool AktifMi { get; set; } = true;
	}
}
