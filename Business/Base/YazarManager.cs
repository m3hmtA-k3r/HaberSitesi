using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	/// <summary>
	/// Business logic for managing Yazarlar (Authors)
	/// Now uses Unit of Work pattern for better transaction management
	/// </summary>
	public class YazarManager : IYazarService
	{
		private readonly IUnitOfWork _unitOfWork;

		public YazarManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public bool DeleteYazar(int id)
		{
			var result = _unitOfWork.YazarlarRepository.Delete(new Yazarlar { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public YazarlarDto GetYazarByEmailPassword(string email, string password)
		{
			var data = _unitOfWork.YazarlarRepository.GetAll();
			var findedData = data.Where(x => x.Eposta == email && x.Sifre == password).FirstOrDefault();
			if (findedData != null)
				return YazarItem(findedData);
			else
				return null;
		}

		public YazarlarDto GetYazarById(int id)
		{
			var response = _unitOfWork.YazarlarRepository.GetById(id);
			return YazarItem(response);
		}

		public List<YazarlarDto> GetYazarlar()
		{
			var response = _unitOfWork.YazarlarRepository.GetAll().ToList();
			List<YazarlarDto> result = new List<YazarlarDto>();

			foreach (var item in response)
				result.Add(YazarItem(item));

			return result;
		}

		public YazarlarDto InsertYazar(YazarlarDto model)
		{
			var response = _unitOfWork.YazarlarRepository.Insert(YazarItem(model));
			_unitOfWork.SaveChanges();

			return YazarItem(response);
		}

		public YazarlarDto UpdateYazar(YazarlarDto model)
		{
			var Yazar = _unitOfWork.YazarlarRepository.GetById(model.Id);
			Yazar.Id = model.Id;
			Yazar.Ad = model.Ad;
			Yazar.Soyad = model.Soyad;
			Yazar.Sifre = model.Sifre;
			Yazar.Eposta = model.Eposta;
			Yazar.Resim = model.Resim;
			Yazar.Aktifmi = model.Aktifmi;
			var response = _unitOfWork.YazarlarRepository.Update(Yazar);
			_unitOfWork.SaveChanges();

			return YazarItem(response);
		}

		private YazarlarDto YazarItem(Yazarlar model)
		{
			YazarlarDto result = new YazarlarDto();
			result.Id = model.Id;
			result.Ad = model.Ad;
			result.Soyad = model.Soyad;
			result.Sifre = model.Sifre;
			result.Eposta = model.Eposta;
			result.Resim = model.Resim;
			result.Aktifmi = model.Aktifmi;
			return result;
		}
		private Yazarlar YazarItem(YazarlarDto model)
		{
			Yazarlar result = new Yazarlar();
			result.Id = model.Id;
			result.Ad = model.Ad;
			result.Soyad = model.Soyad;
			result.Sifre = model.Sifre;
			result.Eposta = model.Eposta;
			result.Resim = model.Resim;
			result.Aktifmi = model.Aktifmi;
			return result;
		}
	}
}
