using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Haberler.Queries.GetAllHaberler
{
	/// <summary>
	/// Handler for GetAllHaberlerQuery
	/// Retrieves all news articles with related data
	/// </summary>
	public class GetAllHaberlerQueryHandler : IRequestHandler<GetAllHaberlerQuery, List<HaberlerDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllHaberlerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<HaberlerDto>> Handle(GetAllHaberlerQuery request, CancellationToken cancellationToken)
		{
			var haberler = _unitOfWork.HaberlerRepository.GetAll().ToList();
			var dtoList = new List<HaberlerDto>();

			foreach (var haber in haberler)
			{
				var dto = _mapper.Map<HaberlerDto>(haber);

				// Get related data
				var yazar = _unitOfWork.YazarlarRepository.GetById(haber.YazarId);
				var kategori = _unitOfWork.KategorilerRepository.GetById(haber.KategoriId);

				dto.Yazar = yazar != null ? $"{yazar.Ad} {yazar.Soyad}" : "Bilinmeyen Yazar";
				dto.YazarResim = yazar?.Resim ?? "";
				dto.Kategori = kategori?.Aciklama ?? "Kategori Yok";

				dtoList.Add(dto);
			}

			return await Task.FromResult(dtoList);
		}
	}
}
