using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class GirisViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir")]
        [EmailAddress(ErrorMessage = "Gecerli bir e-posta adresi giriniz")]
        public string Eposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sifre gereklidir")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; } = string.Empty;
    }
}
