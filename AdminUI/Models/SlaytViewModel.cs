using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminUI.Models
{
	public class SlaytViewModel
	{
		public int Id { get; set; }
		public string Baslik { get; set; } = string.Empty;
		public string Icerik { get; set; } = string.Empty;
		public int HaberId { get; set; }
		public string Haber { get; set; } = string.Empty;
		public string Resim { get; set; } = string.Empty;
		public IFormFile? ResimFile { get; set; }
		public bool Aktifmi { get; set; }
		public List<SelectListItem>? Haberler { get; set; }
	}
}
