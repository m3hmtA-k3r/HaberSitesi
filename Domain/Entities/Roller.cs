using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain entity for roles
    /// </summary>
    [Table("ROLLER")]
    public class Roller
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ROL_ADI")]
        [Required]
        [MaxLength(50)]
        public string RolAdi { get; set; } = string.Empty;

        [Column("ACIKLAMA")]
        [MaxLength(255)]
        public string? Aciklama { get; set; }

        [Column("AKTIF_MI")]
        public bool AktifMi { get; set; } = true;
    }
}
