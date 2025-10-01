using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Dtos
{
	public class SlaytlarDto
	{
		public int Id { get; set; }
		public string Baslik { get; set; }
		public string Icerik { get; set; }
		public int HaberId { get; set; }
		public string? Haber { get; set; }
		public string Resim { get; set; }
		public bool Aktifmi { get; set; }
	}
}
