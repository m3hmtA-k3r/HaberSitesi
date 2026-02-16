using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	[Table("SISTEM_LOGLARI")]
	public class SistemLog
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("KULLANICI_ID")]
		public int? KullaniciId { get; set; }

		[Column("KULLANICI_ADI")]
		[MaxLength(100)]
		public string? KullaniciAdi { get; set; }

		[Column("ISLEM_TIPI")]
		[Required]
		[MaxLength(50)]
		public string IslemTipi { get; set; } = string.Empty;

		[Column("MODUL")]
		[Required]
		[MaxLength(100)]
		public string Modul { get; set; } = string.Empty;

		[Column("ACIKLAMA")]
		[Required]
		public string Aciklama { get; set; } = string.Empty;

		[Column("IP_ADRESI")]
		[MaxLength(45)]
		public string? IpAdresi { get; set; }

		[Column("TARIH")]
		public DateTime Tarih { get; set; } = DateTime.UtcNow;

		[Column("SEVIYE")]
		[Required]
		[MaxLength(20)]
		public string Seviye { get; set; } = "Info";
	}
}
