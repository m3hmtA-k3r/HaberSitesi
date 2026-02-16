using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
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
	}
}