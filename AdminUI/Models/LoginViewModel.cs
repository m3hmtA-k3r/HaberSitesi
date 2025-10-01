using System.ComponentModel.DataAnnotations;

namespace AdminUI.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage ="E-Posta alanı boş geçilemez.")]
        public required string Email { get; set; }
		[Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
		public required string Password { get; set; }
	}
}
