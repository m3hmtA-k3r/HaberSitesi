using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for authors/writers
	/// </summary>
	[Table("YAZARLAR")]
	public class Yazarlar
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("AD")]
		public string Ad { get; set; } = string.Empty;

		[Column("SOYAD")]
		public string Soyad { get; set; } = string.Empty;

		[Column("EPOSTA")]
		public string Eposta { get; set; } = string.Empty;

		[Column("SIFRE")]
		public string Sifre { get; set; } = string.Empty;

		[Column("RESIM")]
		public string Resim { get; set; } = string.Empty;

		[Column("AKTIF_MI")]
		public bool Aktifmi { get; set; }

		[Column("KULLANICI_ID")]
		public int? KullaniciId { get; set; }
	}
}
