using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
	[Table("HABERLER")]
	public class Haberler
	{
        [Key]
		[Column("ID")]
        public int Id { get; set; }
		[Column("BASLIK")]
		public string Baslik { get; set; }
		[Column("EKLEME_TARIHI")]
		public DateTime EklenmeTarihi { get; set; }
		[Column("YAZAR_ID")]
		public int YazarId { get; set; }
		[Column("CATEGORY_ID")]
		public int KategoriId { get; set; }
		[Column("ICERIK")]
        public string Icerik { get; set; }
		[Column("RESIM")]
		public string Resim { get; set; }

		[Column("VIDEO")]
		public string Video { get; set; }
		[Column("GOSTERIM_SAYISI")]
		public int GosterimSayisi { get; set; }
		[Column("AKTIF_MI")]
		public bool Aktifmi { get; set; }
	}
}
