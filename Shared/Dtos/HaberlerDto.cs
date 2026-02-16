namespace Shared.Dtos
{
	public class HaberlerDto
	{
		public int Id { get; set; }
		public string Baslik { get; set; } = string.Empty;
		public DateTime EklenmeTarihi { get; set; }
		public int YazarId { get; set; }
		public string? Yazar {  get; set; }
        public string? YazarResim { get; set; }
        public int KategoriId { get; set; }
		public string? Kategori { get; set; }
		public string Icerik { get; set; } = string.Empty;
		public string Resim { get; set; } = string.Empty;
		public string Video { get; set; } = string.Empty;
		public int GosterimSayisi { get; set; }
		public bool Aktifmi { get; set; }
	}
}
