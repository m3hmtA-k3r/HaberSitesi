using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for contact form messages
	/// </summary>
	[Table("ILETISIM_MESAJLARI")]
	public class IletisimMesajlari
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("AD")]
		[Required]
		[MaxLength(100)]
		public string Ad { get; set; } = string.Empty;

		[Column("EPOSTA")]
		[Required]
		[MaxLength(255)]
		public string Eposta { get; set; } = string.Empty;

		[Column("KONU")]
		[Required]
		[MaxLength(200)]
		public string Konu { get; set; } = string.Empty;

		[Column("MESAJ")]
		[Required]
		public string Mesaj { get; set; } = string.Empty;

		[Column("IP_ADRESI")]
		[MaxLength(45)]
		public string? IpAdresi { get; set; }

		[Column("EKLEME_TARIHI")]
		public DateTime EklemeTarihi { get; set; } = DateTime.UtcNow;

		[Column("OKUNDU_MU")]
		public bool OkunduMu { get; set; } = false;

		[Column("CEVAPLANDI_MI")]
		public bool CevaplandiMi { get; set; } = false;

		[Column("CEVAP_TARIHI")]
		public DateTime? CevapTarihi { get; set; }
	}
}
