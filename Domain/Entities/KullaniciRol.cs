using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain entity for user-role mapping (many-to-many relationship)
    /// </summary>
    [Table("KULLANICI_ROLLER")]
    public class KullaniciRol
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("KULLANICI_ID")]
        [Required]
        public int KullaniciId { get; set; }

        [Column("ROL_ID")]
        [Required]
        public int RolId { get; set; }

        [Column("ATANMA_TARIHI")]
        public DateTime AtanmaTarihi { get; set; } = DateTime.UtcNow;
    }
}
