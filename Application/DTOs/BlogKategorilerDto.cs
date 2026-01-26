namespace Application.DTOs
{
	public class BlogKategorilerDto
	{
		public int Id { get; set; }
		public string Adi { get; set; } = string.Empty;
		public string? Aciklama { get; set; }
		public int Sira { get; set; }
		public bool AktifMi { get; set; }
		public DateTime OlusturmaTarihi { get; set; }
		public int BlogSayisi { get; set; }
	}
}
