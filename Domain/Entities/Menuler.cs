using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain entity for menu groups
    /// </summary>
    [Table("MENULER")]
    public class Menuler
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ADI")]
        [Required]
        [MaxLength(100)]
        public string Adi { get; set; } = string.Empty;

        [Column("IKON")]
        [MaxLength(100)]
        public string? Ikon { get; set; }

        [Column("SIRA")]
        public int Sira { get; set; } = 0;

        [Column("AKTIF_MI")]
        public bool AktifMi { get; set; } = true;

        [Column("COLLAPSE_ID")]
        [MaxLength(50)]
        public string? CollapseId { get; set; }

        // Navigation property
        public virtual ICollection<MenuOgeleri>? MenuOgeleri { get; set; }
    }
}
