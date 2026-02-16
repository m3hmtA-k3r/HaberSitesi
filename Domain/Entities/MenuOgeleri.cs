using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain entity for menu items
    /// </summary>
    [Table("MENU_OGELERI")]
    public class MenuOgeleri
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("MENU_ID")]
        public int? MenuId { get; set; }

        [Column("ADI")]
        [Required]
        [MaxLength(100)]
        public string Adi { get; set; } = string.Empty;

        [Column("URL")]
        [MaxLength(255)]
        public string? Url { get; set; }

        [Column("IKON")]
        [MaxLength(100)]
        public string? Ikon { get; set; }

        [Column("SIRA")]
        public int Sira { get; set; } = 0;

        [Column("AKTIF_MI")]
        public bool AktifMi { get; set; } = true;

        // Navigation property
        [ForeignKey("MenuId")]
        public virtual Menuler? Menu { get; set; }
    }
}
