using Application.DTOs;
using MediatR;

namespace Application.Features.Haberler.Commands.CreateHaber
{
	/// <summary>
	/// Command to create a new Haber (News Article)
	/// Uses CQRS pattern with MediatR
	/// </summary>
	public class CreateHaberCommand : IRequest<HaberlerDto>
	{
		public string Baslik { get; set; }
		public int YazarId { get; set; }
		public int KategoriId { get; set; }
		public string Icerik { get; set; }
		public string Resim { get; set; }
		public string Video { get; set; }
		public bool Aktifmi { get; set; }
	}
}
