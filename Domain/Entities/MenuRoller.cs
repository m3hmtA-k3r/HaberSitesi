using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain entity for menu-role mapping (many-to-many relationship)
    /// </summary>
    [Table("MENU_ROLLER")]
    public class MenuRoller
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("MENU_ID")]
        [Required]
        public int MenuId { get; set; }

        [Column("ROL_ID")]
        [Required]
        public int RolId { get; set; }

        // Navigation properties
        [ForeignKey("MenuId")]
        public virtual Menuler? Menu { get; set; }

        [ForeignKey("RolId")]
        public virtual Roller? Rol { get; set; }
    }
}
