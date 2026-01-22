using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	/// <summary>
	/// Business logic for managing Yorumlar (Comments)
	/// Now uses Unit of Work pattern for better transaction management
	/// </summary>
	public class YorumManager : IYorumService
	{
		private readonly IUnitOfWork _unitOfWork;

		public YorumManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public int GetOnayBekleyenYorumSayisi()
		{
			return _unitOfWork.YorumlarRepository.GetAll().Count(yorum => yorum.Aktifmi == false);
		}

		public bool DeleteYorum(int id)
		{
			var result = _unitOfWork.YorumlarRepository.Delete(new Yorumlar { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public YorumlarDto GetYorumById(int id)
		{
			var response = _unitOfWork.YorumlarRepository.GetById(id);
			return YorumItem(response);
		}

		public List<YorumlarDto> GetYorumlar()
		{
			var response = _unitOfWork.YorumlarRepository.GetAll().ToList();
			List<YorumlarDto> result = new List<YorumlarDto>();

			foreach (var item in response)
				result.Add(YorumItem(item));

			return result;
		}

		public YorumlarDto InsertYorum(YorumlarDto model)
		{
			model.EklenmeTarihi = DateTime.Now;
			var response = _unitOfWork.YorumlarRepository.Insert(YorumItem(model));
			_unitOfWork.SaveChanges();

			return YorumItem(response);
		}

		public YorumlarDto UpdateYorum(YorumlarDto model)
		{
			var yorum = _unitOfWork.YorumlarRepository.GetById(model.Id);
			yorum.Id = model.Id;
			yorum.Ad = model.Ad;
			yorum.Soyad = model.Soyad;
			yorum.HaberId = model.HaberId;
			yorum.Eposta = model.Eposta;
			yorum.Baslik = model.Baslik;
			yorum.Icerik = model.Icerik;
			yorum.Aktifmi = model.Aktifmi;
			var response = _unitOfWork.YorumlarRepository.Update(yorum);
			_unitOfWork.SaveChanges();

			return YorumItem(response);
		}

		private YorumlarDto YorumItem(Yorumlar model)
		{
			YorumlarDto result = new YorumlarDto();
			result.Id = model.Id;
			result.Ad = model.Ad;
			result.Soyad = model.Soyad;
			result.HaberId = model.HaberId;
			result.Eposta = model.Eposta;
			result.Baslik = model.Baslik;
			result.Icerik = model.Icerik;
			result.EklenmeTarihi = model.EklenmeTarihi;
			result.Aktifmi = model.Aktifmi;

			var haber = _unitOfWork.HaberlerRepository.GetById(model.HaberId);
			result.HaberBaslik = haber?.Baslik ?? "Haber Bulunamadı";
			return result;
		}
		private Yorumlar YorumItem(YorumlarDto model)
		{
			Yorumlar result = new Yorumlar();
			result.Id = model.Id;
			result.Ad = model.Ad;
			result.Soyad = model.Soyad;
			result.HaberId = model.HaberId;
			result.Eposta = model.Eposta;
			result.Baslik = model.Baslik;
			result.Icerik = model.Icerik;
			result.EklenmeTarihi = model.EklenmeTarihi;
			result.Aktifmi = model.Aktifmi;
			return result;
		}
	}
}
