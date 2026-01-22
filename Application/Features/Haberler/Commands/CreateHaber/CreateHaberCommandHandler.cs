using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Haberler.Commands.CreateHaber
{
	/// <summary>
	/// Handler for CreateHaberCommand
	/// Implements CQRS pattern with MediatR
	/// </summary>
	public class CreateHaberCommandHandler : IRequestHandler<CreateHaberCommand, HaberlerDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CreateHaberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<HaberlerDto> Handle(CreateHaberCommand request, CancellationToken cancellationToken)
		{
			// Map command to entity
			var haber = new Domain.Entities.Haberler
			{
				Baslik = request.Baslik,
				Icerik = request.Icerik,
				YazarId = request.YazarId,
				KategoriId = request.KategoriId,
				Resim = request.Resim,
				Video = request.Video,
				Aktifmi = request.Aktifmi,
				EklenmeTarihi = DateTime.Now,
				GosterimSayisi = 0
			};

			// Insert using Unit of Work
			var result = _unitOfWork.HaberlerRepository.Insert(haber);
			await _unitOfWork.SaveChangesAsync();

			// Get related data for DTO
			var yazar = _unitOfWork.YazarlarRepository.GetById(result.YazarId);
			var kategori = _unitOfWork.KategorilerRepository.GetById(result.KategoriId);

			// Map to DTO
			var dto = _mapper.Map<HaberlerDto>(result);
			dto.Yazar = yazar != null ? $"{yazar.Ad} {yazar.Soyad}" : "Bilinmeyen Yazar";
			dto.YazarResim = yazar?.Resim ?? "";
			dto.Kategori = kategori?.Aciklama ?? "Kategori Yok";

			return dto;
		}
	}
}
