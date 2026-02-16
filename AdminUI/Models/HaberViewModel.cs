using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminUI.Models
{
	public class HaberViewModel
	{
		public int Id { get; set; }
		public string Baslik { get; set; } = string.Empty;
		public DateTime? EklenmeTarihi { get; set; }
		public int YazarId { get; set; }
		public int KategoriId { get; set; }
		public string Icerik { get; set; } = string.Empty;
		public string Resim { get; set; } = string.Empty;
		public IFormFile? ResimFile { get; set; }
		public string Video { get; set; } = string.Empty;
		public IFormFile? VideoFile { get; set; }
		public int GosterimSayisi { get; set; }
		public bool Aktifmi { get; set; }

		public List<SelectListItem>? Yazarlar { get; set; }
		public List<SelectListItem>? Kategoriler { get; set; }
	}
}
