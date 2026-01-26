namespace Shared.Dtos
{
	public class BloglarDto
	{
		public int Id { get; set; }
		public string Baslik { get; set; } = string.Empty;
		public string? Ozet { get; set; }
		public string Icerik { get; set; } = string.Empty;
		public string? GorselUrl { get; set; }
		public string? Etiketler { get; set; }
		public List<string> EtiketListesi => string.IsNullOrEmpty(Etiketler)
			? new List<string>()
			: Etiketler.Split(',').Select(e => e.Trim()).ToList();
		public DateTime YayinTarihi { get; set; }
		public DateTime OlusturmaTarihi { get; set; }
		public DateTime? GuncellenmeTarihi { get; set; }
		public bool AktifMi { get; set; }
		public int? KategoriId { get; set; }
		public string? KategoriAdi { get; set; }
		public int? YazarId { get; set; }
		public string? YazarAdi { get; set; }
		public string? YazarResim { get; set; }
		public int GoruntulenmeSayisi { get; set; }
	}

	public class BlogCreateDto
	{
		public string Baslik { get; set; } = string.Empty;
		public string? Ozet { get; set; }
		public string Icerik { get; set; } = string.Empty;
		public string? GorselUrl { get; set; }
		public string? Etiketler { get; set; }
		public DateTime? YayinTarihi { get; set; }
		public bool AktifMi { get; set; } = true;
		public int? KategoriId { get; set; }
		public int? YazarId { get; set; }
	}

	public class BlogUpdateDto
	{
		public int Id { get; set; }
		public string Baslik { get; set; } = string.Empty;
		public string? Ozet { get; set; }
		public string Icerik { get; set; } = string.Empty;
		public string? GorselUrl { get; set; }
		public string? Etiketler { get; set; }
		public DateTime? YayinTarihi { get; set; }
		public bool AktifMi { get; set; }
		public int? KategoriId { get; set; }
		public int? YazarId { get; set; }
	}
}
