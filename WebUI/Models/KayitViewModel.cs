using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class KayitViewModel
    {
        [Required(ErrorMessage = "Ad gereklidir")]
        [MaxLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad gereklidir")]
        [MaxLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi gereklidir")]
        [EmailAddress(ErrorMessage = "Gecerli bir e-posta adresi giriniz")]
        public string Eposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sifre gereklidir")]
        [MinLength(6, ErrorMessage = "Sifre en az 6 karakter olmalidir")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sifre tekrari gereklidir")]
        [Compare("Sifre", ErrorMessage = "Sifreler eslesmiyor")]
        [DataType(DataType.Password)]
        public string SifreTekrar { get; set; } = string.Empty;
    }
}
