using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminUI.Models
{
	public class SlaytViewModel
	{
		public int Id { get; set; }
		public string Baslik { get; set; }
		public string Icerik { get; set; }
		public int HaberId { get; set; }
		public string Haber { get; set; }
		public string Resim { get; set; }
		public IFormFile? ResimFile { get; set; }
		public bool Aktifmi { get; set; }
		public List<SelectListItem>? Haberler { get; set; }
	}
}
