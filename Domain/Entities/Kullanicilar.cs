using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain entity for users
    /// </summary>
    [Table("KULLANICILAR")]
    public class Kullanicilar
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("AD")]
        [Required]
        [MaxLength(100)]
        public string Ad { get; set; } = string.Empty;

        [Column("SOYAD")]
        [Required]
        [MaxLength(100)]
        public string Soyad { get; set; } = string.Empty;

        [Column("EPOSTA")]
        [Required]
        [MaxLength(255)]
        public string Eposta { get; set; } = string.Empty;

        [Column("SIFRE_HASH")]
        [Required]
        public string SifreHash { get; set; } = string.Empty;

        [Column("RESIM")]
        [MaxLength(500)]
        public string? Resim { get; set; }

        [Column("AKTIF_MI")]
        public bool AktifMi { get; set; } = true;

        [Column("OLUSTURMA_TARIHI")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        [Column("SON_GIRIS_TARIHI")]
        public DateTime? SonGirisTarihi { get; set; }
    }
}
