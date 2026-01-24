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
