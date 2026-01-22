using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for comments
	/// </summary>
	[Table("YORUMLAR")]
	public class Yorumlar
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

		[Column("BASLIK")]
		public string Baslik { get; set; }

		[Column("ICERIK")]
		public string Icerik { get; set; }

		[Column("HABER_ID")]
		public int HaberId { get; set; }

		[Column("EKLEME_TARIHI")]
		public DateTime EklenmeTarihi { get; set; }

		[Column("AKTIF_MI")]
		public bool Aktifmi { get; set; }
	}
}
