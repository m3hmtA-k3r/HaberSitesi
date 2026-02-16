using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
	[Table("KATEGORILER")]
	public class Kategoriler
	{
        [Key]
        [Column("ID")]
        public int Id { get; set; }
		[Column("ACIKLAMA")]
		public string Aciklama { get; set; } = string.Empty;

		[Column("AKTIF_MI")]
		public bool Aktifmi { get; set; }
    }
}