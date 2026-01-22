namespace Application.DTOs
{
	/// <summary>
	/// Data Transfer Object for Haberler (News)
	/// </summary>
	public class HaberlerDto
	{
		public int Id { get; set; }
		public string Baslik { get; set; }
		public DateTime EklenmeTarihi { get; set; }
		public int YazarId { get; set; }
		public string? Yazar { get; set; }
		public string? YazarResim { get; set; }
		public int KategoriId { get; set; }
		public string? Kategori { get; set; }
		public string Icerik { get; set; }
		public string Resim { get; set; }
		public string Video { get; set; }
		public int GosterimSayisi { get; set; }
		public bool Aktifmi { get; set; }
	}
}
