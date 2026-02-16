using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Dtos
{
	public class SlaytlarDto
	{
		public int Id { get; set; }
		public string Baslik { get; set; } = string.Empty;
		public string Icerik { get; set; } = string.Empty;
		public int HaberId { get; set; }
		public string? Haber { get; set; }
		public string Resim { get; set; } = string.Empty;
		public bool Aktifmi { get; set; }
	}
}
