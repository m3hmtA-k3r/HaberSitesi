namespace Shared.Dtos
{
	public class BlogYorumlarDto
	{
		public int Id { get; set; }
		public int BlogId { get; set; }
		public string? BlogBaslik { get; set; }
		public string Ad { get; set; } = string.Empty;
		public string? Soyad { get; set; }
		public string AdSoyad => $"{Ad} {Soyad}".Trim();
		public string Eposta { get; set; } = string.Empty;
		public string Yorum { get; set; } = string.Empty;
		public bool OnaylandiMi { get; set; }
		public bool AktifMi { get; set; }
		public DateTime OlusturmaTarihi { get; set; }
	}

	public class BlogYorumCreateDto
	{
		public int BlogId { get; set; }
		public string Ad { get; set; } = string.Empty;
		public string? Soyad { get; set; }
		public string Eposta { get; set; } = string.Empty;
		public string Yorum { get; set; } = string.Empty;
	}

	public class BlogYorumUpdateDto
	{
		public int Id { get; set; }
		public string Ad { get; set; } = string.Empty;
		public string? Soyad { get; set; }
		public string Eposta { get; set; } = string.Empty;
		public string Yorum { get; set; } = string.Empty;
		public bool OnaylandiMi { get; set; }
		public bool AktifMi { get; set; }
	}
}
