using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for news categories
	/// </summary>
	[Table("KATEGORILER")]
	public class Kategoriler
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("ACIKLAMA")]
		public string Aciklama { get; set; }

		[Column("AKTIF_MI")]
		public bool Aktifmi { get; set; }
	}
}
