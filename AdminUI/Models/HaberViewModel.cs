using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminUI.Models
{
	public class HaberViewModel
	{
		public int Id { get; set; }
		public string Baslik { get; set; }
		public DateTime? EklenmeTarihi { get; set; }
		public int YazarId { get; set; }
		public int KategoriId { get; set; }
		public string Icerik { get; set; }
		public string Resim { get; set; }
		public IFormFile? ResimFile { get; set; }
		public string Video { get; set; }
		public IFormFile? VideoFile { get; set; }
		public int GosterimSayisi { get; set; }
		public bool Aktifmi { get; set; }

		public List<SelectListItem>? Yazarlar { get; set; }
		public List<SelectListItem>? Kategoriler { get; set; }
	}
}
