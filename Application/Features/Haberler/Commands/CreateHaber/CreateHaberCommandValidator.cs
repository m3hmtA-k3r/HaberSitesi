using FluentValidation;

namespace Application.Features.Haberler.Commands.CreateHaber
{
	/// <summary>
	/// Validation rules for CreateHaberCommand
	/// Uses FluentValidation for robust input validation
	/// </summary>
	public class CreateHaberCommandValidator : AbstractValidator<CreateHaberCommand>
	{
		public CreateHaberCommandValidator()
		{
			RuleFor(x => x.Baslik)
				.NotEmpty().WithMessage("Başlık boş olamaz")
				.MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir");

			RuleFor(x => x.Icerik)
				.NotEmpty().WithMessage("İçerik boş olamaz")
				.MinimumLength(50).WithMessage("İçerik en az 50 karakter olmalıdır");

			RuleFor(x => x.YazarId)
				.GreaterThan(0).WithMessage("Geçerli bir yazar seçilmelidir");

			RuleFor(x => x.KategoriId)
				.GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir");

			RuleFor(x => x.Resim)
				.NotEmpty().WithMessage("Resim alanı boş olamaz");
		}
	}
}
