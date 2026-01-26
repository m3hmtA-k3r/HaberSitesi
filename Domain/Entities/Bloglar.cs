using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	/// <summary>
	/// Domain entity for blog posts
	/// Referans: 2.sitem/src/data/BlogData.js
	/// </summary>
	[Table("BLOGLAR")]
	public class Bloglar
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("BASLIK")]
		[Required]
		[MaxLength(300)]
		public string Baslik { get; set; } = string.Empty;

		[Column("OZET")]
		[MaxLength(500)]
		public string? Ozet { get; set; }

		[Column("ICERIK")]
		public string Icerik { get; set; } = string.Empty;

		[Column("GORSEL_URL")]
		[MaxLength(500)]
		public string? GorselUrl { get; set; }

		[Column("ETIKETLER")]
		[MaxLength(500)]
		public string? Etiketler { get; set; }  // Virgul ile ayrilmis: "React,CSS,JavaScript"

		[Column("YAYIN_TARIHI", TypeName = "timestamp without time zone")]
		public DateTime YayinTarihi { get; set; }

		[Column("OLUSTURMA_TARIHI", TypeName = "timestamp without time zone")]
		public DateTime OlusturmaTarihi { get; set; }

		[Column("GUNCELLENME_TARIHI", TypeName = "timestamp without time zone")]
		public DateTime? GuncellenmeTarihi { get; set; }

		[Column("AKTIF_MI")]
		public bool AktifMi { get; set; } = true;

		[Column("KATEGORI_ID")]
		public int? KategoriId { get; set; }

		[Column("YAZAR_ID")]
		public int? YazarId { get; set; }

		[Column("GORUNTULEME_SAYISI")]
		public int GoruntulenmeSayisi { get; set; } = 0;

		// Navigation properties
		[ForeignKey("KategoriId")]
		public virtual BlogKategoriler? Kategori { get; set; }

		[ForeignKey("YazarId")]
		public virtual Kullanicilar? Yazar { get; set; }

		public virtual ICollection<BlogYorumlar>? Yorumlar { get; set; }
	}
}
