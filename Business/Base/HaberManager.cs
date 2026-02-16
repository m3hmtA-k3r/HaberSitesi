using Application.DTOs;
using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;

namespace Business.Base
{
	/// <summary>
	/// Business logic for managing Haberler (News)
	/// Now uses Unit of Work pattern for better transaction management
	/// </summary>
	public class HaberManager : IHaberService
	{
		private readonly IUnitOfWork _unitOfWork;

		public HaberManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public bool DeleteHaber(int id)
		{
			var result = _unitOfWork.HaberlerRepository.Delete(new Haberler { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public HaberlerDto GetHaberById(int id)
		{
			var response = _unitOfWork.HaberlerRepository.GetById(id);
			return HaberItem(response);
		}

		public List<HaberlerDto> GetHaberler()
		{
			var response = _unitOfWork.HaberlerRepository.GetAll().ToList();

			List<HaberlerDto> result = [];

			foreach (var item in response)
				result.Add(HaberItem(item));

			return result;
		}

		public PagedResultDto<HaberlerDto> GetHaberlerPaged(int page = 1, int pageSize = 9, bool? aktif = null, int? kategoriId = null, string siralama = "yeni")
		{
			var query = _unitOfWork.HaberlerRepository.Query();

			// Aktiflik filtresi
			if (aktif.HasValue)
				query = query.Where(x => x.Aktifmi == aktif.Value);

			// Kategori filtresi
			if (kategoriId.HasValue && kategoriId.Value > 0)
				query = query.Where(x => x.KategoriId == kategoriId.Value);

			// Siralama
			query = siralama switch
			{
				"eski" => query.OrderBy(x => x.EklenmeTarihi),
				"populer" => query.OrderByDescending(x => x.GosterimSayisi),
				"az" => query.OrderBy(x => x.GosterimSayisi),
				_ => query.OrderByDescending(x => x.EklenmeTarihi)
			};

			// Toplam kayit sayisi (veritabaninda hesaplanir)
			var totalCount = query.Count();

			// Sayfalama (veritabaninda Skip/Take)
			var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

			// Yazar ve kategori bilgilerini toplu yukle (N+1 sorgu onleme)
			var yazarIds = items.Select(x => x.YazarId).Distinct().ToList();
			var kategoriIds = items.Select(x => x.KategoriId).Distinct().ToList();

			var yazarlar = _unitOfWork.YazarlarRepository.Query()
				.Where(y => yazarIds.Contains(y.Id))
				.ToDictionary(y => y.Id);

			var kategoriler = _unitOfWork.KategorilerRepository.Query()
				.Where(k => kategoriIds.Contains(k.Id))
				.ToDictionary(k => k.Id);

			// DTO'ya donustur
			var dtoItems = items.Select(item =>
			{
				var dto = new HaberlerDto
				{
					Id = item.Id,
					Baslik = item.Baslik,
					Icerik = item.Icerik,
					Aktifmi = item.Aktifmi,
					Resim = item.Resim,
					EklenmeTarihi = DateTime.SpecifyKind(item.EklenmeTarihi, DateTimeKind.Utc),
					YazarId = item.YazarId,
					KategoriId = item.KategoriId,
					GosterimSayisi = item.GosterimSayisi,
					Video = item.Video
				};

				if (yazarlar.TryGetValue(item.YazarId, out var yazar))
				{
					dto.Yazar = yazar.Ad + " " + yazar.Soyad;
					dto.YazarResim = yazar.Resim;
				}
				else
				{
					dto.Yazar = "Bilinmeyen Yazar";
					dto.YazarResim = "";
				}

				dto.Kategori = kategoriler.TryGetValue(item.KategoriId, out var kat) ? kat.Aciklama ?? "Kategori Yok" : "Kategori Yok";

				return dto;
			}).ToList();

			return new PagedResultDto<HaberlerDto>
			{
				Items = dtoItems,
				TotalCount = totalCount,
				Page = page,
				PageSize = pageSize
			};
		}

		public HaberlerDto InsertHaber(HaberlerDto model)
		{
			model.EklenmeTarihi = DateTime.UtcNow;
			Haberler response = _unitOfWork.HaberlerRepository.Insert(HaberItem(model));
			_unitOfWork.SaveChanges();

			return HaberItem(response);
		}

		public HaberlerDto UpdateHaber(HaberlerDto model)
		{
			var haber = _unitOfWork.HaberlerRepository.GetById(model.Id);
			haber.Id = model.Id;
			haber.Baslik = model.Baslik;
			haber.Icerik = model.Icerik;
			haber.Aktifmi = model.Aktifmi;
			haber.Resim = model.Resim;
			haber.YazarId = model.YazarId;
			haber.KategoriId = model.KategoriId;
			haber.Video = model.Video;

			Haberler response = _unitOfWork.HaberlerRepository.Update(haber);
			_unitOfWork.SaveChanges();

			return HaberItem(response);
		}

		private HaberlerDto HaberItem(Haberler model)
		{
			if (model == null)
				return null;

			var yazar = _unitOfWork.YazarlarRepository.GetById(model.YazarId);
			var kategori = _unitOfWork.KategorilerRepository.GetById(model.KategoriId);

			HaberlerDto result = new HaberlerDto();
			result.Id = model.Id;
			result.Baslik = model.Baslik;
			result.Icerik = model.Icerik;
			result.Aktifmi = model.Aktifmi;
			result.Resim = model.Resim;
			result.EklenmeTarihi = DateTime.SpecifyKind(model.EklenmeTarihi, DateTimeKind.Utc);
			result.YazarId = model.YazarId;

			if (yazar != null)
			{
				result.Yazar = yazar.Ad + " " + yazar.Soyad;
				result.YazarResim = yazar.Resim;
			}
			else
			{
				result.Yazar = "Bilinmeyen Yazar";
				result.YazarResim = "";
			}

			result.KategoriId = model.KategoriId;
			result.Kategori = kategori?.Aciklama ?? "Kategori Yok";
			result.GosterimSayisi = model.GosterimSayisi;
			result.Video = model.Video;
			return result;
		}
		private Haberler HaberItem(HaberlerDto model)
		{
			Haberler result = new Haberler();
			result.Id = model.Id;
			result.Baslik = model.Baslik;
			result.Icerik = model.Icerik;
			result.Aktifmi = model.Aktifmi;
			result.Resim = model.Resim;
			result.EklenmeTarihi = DateTime.SpecifyKind(model.EklenmeTarihi, DateTimeKind.Utc);
			result.YazarId = model.YazarId;
			result.KategoriId = model.KategoriId;
			result.GosterimSayisi = model.GosterimSayisi;
			result.Video = model.Video;
			return result;
		}
	}
}
