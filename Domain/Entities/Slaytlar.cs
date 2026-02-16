using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for slider/carousel items
	/// </summary>
	[Table("SLAYTLAR")]
	public class Slaytlar
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("BASLIK")]
		public string Baslik { get; set; } = string.Empty;

		[Column("ICERIK")]
		public string Icerik { get; set; } = string.Empty;

		[Column("HABER_ID")]
		public int HaberId { get; set; }

		[Column("RESIM")]
		public string Resim { get; set; } = string.Empty;

		[Column("AKTIF_MI")]
		public bool Aktifmi { get; set; }
	}
}
