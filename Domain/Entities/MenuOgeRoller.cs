using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain entity for menu item-role mapping (many-to-many relationship)
    /// </summary>
    [Table("MENU_OGELERI_ROLLER")]
    public class MenuOgeRoller
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("MENU_OGE_ID")]
        [Required]
        public int MenuOgeId { get; set; }

        [Column("ROL_ID")]
        [Required]
        public int RolId { get; set; }

        // Navigation properties
        [ForeignKey("MenuOgeId")]
        public virtual MenuOgeleri? MenuOgesi { get; set; }

        [ForeignKey("RolId")]
        public virtual Roller? Rol { get; set; }
    }
}
