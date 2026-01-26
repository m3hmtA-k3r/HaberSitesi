using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for blog comments
	/// </summary>
	[Table("BLOG_YORUMLAR")]
	public class BlogYorumlar
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("BLOG_ID")]
		[Required]
		public int BlogId { get; set; }

		[Column("AD")]
		[Required]
		[MaxLength(100)]
		public string Ad { get; set; } = string.Empty;

		[Column("SOYAD")]
		[MaxLength(100)]
		public string? Soyad { get; set; }

		[Column("EPOSTA")]
		[Required]
		[MaxLength(255)]
		public string Eposta { get; set; } = string.Empty;

		[Column("YORUM")]
		[Required]
		[MaxLength(2000)]
		public string Yorum { get; set; } = string.Empty;

		[Column("ONAYLANDI_MI")]
		public bool OnaylandiMi { get; set; } = false;

		[Column("AKTIF_MI")]
		public bool AktifMi { get; set; } = true;

		[Column("OLUSTURMA_TARIHI", TypeName = "timestamp without time zone")]
		public DateTime OlusturmaTarihi { get; set; }

		// Navigation property
		[ForeignKey("BlogId")]
		public virtual Bloglar? Blog { get; set; }
	}
}
