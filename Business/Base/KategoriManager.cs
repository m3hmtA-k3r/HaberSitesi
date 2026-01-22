using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	/// <summary>
	/// Business logic for managing Kategoriler (Categories)
	/// Now uses Unit of Work pattern for better transaction management
	/// </summary>
	public class KategoriManager : IKategoriService
	{
		private readonly IUnitOfWork _unitOfWork;

		public KategoriManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public bool DeleteKategori(int id)
		{
			var result = _unitOfWork.KategorilerRepository.Delete(new Kategoriler { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public KategorilerDto GetKategoriById(int id)
		{
			var response = _unitOfWork.KategorilerRepository.GetById(id);
			return KategoriItem(response);
		}

		public List<KategorilerDto> GetKategoriler()
		{
			var response = _unitOfWork.KategorilerRepository.GetAll().ToList();

			List<KategorilerDto> result = new List<KategorilerDto>();

			foreach (var item in response)
				result.Add(KategoriItem(item));

			return result;
		}

		public KategorilerDto InsertKategori(KategorilerDto model)
		{
			Kategoriler response = _unitOfWork.KategorilerRepository.Insert(KategoriItem(model));
			_unitOfWork.SaveChanges();

			return KategoriItem(response);
		}

		public KategorilerDto UpdateKategori(KategorilerDto model)
		{
			var kategori = _unitOfWork.KategorilerRepository.GetById(model.Id);
			kategori.Aktifmi = model.Aktifmi;
			kategori.Aciklama = model.Aciklama;
			Kategoriler response = _unitOfWork.KategorilerRepository.Update(kategori);
			_unitOfWork.SaveChanges();

			return KategoriItem(response);
		}

		private KategorilerDto KategoriItem(Kategoriler model)
		{
			KategorilerDto result = new KategorilerDto();
			result.Id = model.Id;
			result.Aciklama = model.Aciklama;
			result.Aktifmi = model.Aktifmi;
			return result;
		}
		private Kategoriler KategoriItem(KategorilerDto model)
		{
			Kategoriler result = new Kategoriler();
			result.Id = model.Id;
			result.Aciklama = model.Aciklama;
			result.Aktifmi = model.Aktifmi;
			return result;
		}
	}
}
