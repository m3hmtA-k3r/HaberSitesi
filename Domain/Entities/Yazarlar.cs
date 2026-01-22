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
		public string Ad { get; set; }

		[Column("SOYAD")]
		public string Soyad { get; set; }

		[Column("EPOSTA")]
		public string Eposta { get; set; }

		[Column("SIFRE")]
		public string Sifre { get; set; }

		[Column("RESIM")]
		public string Resim { get; set; }

		[Column("AKTIF_MI")]
		public bool Aktifmi { get; set; }
	}
}
