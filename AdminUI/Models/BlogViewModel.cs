using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminUI.Models
{
	public class BlogViewModel
	{
		public int Id { get; set; }
		public string Baslik { get; set; } = string.Empty;
		public string? Ozet { get; set; }
		public string Icerik { get; set; } = string.Empty;
		public string? GorselUrl { get; set; }
		public IFormFile? GorselFile { get; set; }
		public string? Etiketler { get; set; }
		public DateTime YayinTarihi { get; set; }
		public DateTime OlusturmaTarihi { get; set; }
		public bool AktifMi { get; set; }
		public int? KategoriId { get; set; }
		public int? YazarId { get; set; }
		public int GoruntulenmeSayisi { get; set; }

		public List<SelectListItem>? Kategoriler { get; set; }
		public List<SelectListItem>? Yazarlar { get; set; }
	}
}
