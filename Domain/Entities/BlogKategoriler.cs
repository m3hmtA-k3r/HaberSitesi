using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for blog categories
	/// </summary>
	[Table("BLOG_KATEGORILER")]
	public class BlogKategoriler
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("ADI")]
		[Required]
		[MaxLength(100)]
		public string Adi { get; set; } = string.Empty;

		[Column("ACIKLAMA")]
		[MaxLength(500)]
		public string? Aciklama { get; set; }

		[Column("SIRA")]
		public int Sira { get; set; } = 0;

		[Column("AKTIF_MI")]
		public bool AktifMi { get; set; } = true;

		[Column("OLUSTURMA_TARIHI", TypeName = "timestamp without time zone")]
		public DateTime OlusturmaTarihi { get; set; }

		// Navigation property
		public virtual ICollection<Bloglar>? Bloglar { get; set; }
	}
}
