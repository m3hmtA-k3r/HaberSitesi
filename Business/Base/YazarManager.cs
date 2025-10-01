using Business.Abstract;
using DataAccess.Abstract.Repository;
using Shared.Dtos;
using Shared.Entities;

namespace Business.Base
{
	public class YazarManager : IYazarService
	{
		private readonly IRepository<Yazarlar> _repository;
		public YazarManager(IRepository<Yazarlar> repository)
		{
			_repository = repository;
		}
		public bool DeleteYazar(int id)
		{
			return _repository.Delete(new Yazarlar { Id = id });
		}

		public YazarlarDto GetYazarByEmailPassword(string email, string password)
		{
			var data = _repository.GetAll();
			var findedData = data.Where(x => x.Eposta == email && x.Sifre == password).FirstOrDefault();
			if (findedData != null)
				return YazarItem(findedData);
			else
				return null;
		}

		public YazarlarDto GetYazarById(int id)
		{
			var response = _repository.GetById(id);

			return YazarItem(response);
		}

		public List<YazarlarDto> GetYazarlar()
		{
			var response = _repository.GetAll().ToList();
			List<YazarlarDto> result = new List<YazarlarDto>();

			foreach (var item in response)
				result.Add(YazarItem(item));

			return result;
		}

		public YazarlarDto InsertYazar(YazarlarDto model)
		{
			var response = _repository.Insert(YazarItem(model));

			return YazarItem(response);
		}

		public YazarlarDto UpdateYazar(YazarlarDto model)
		{
			var Yazar = _repository.GetById(model.Id);
			Yazar.Id = model.Id;
			Yazar.Ad = model.Ad;
			Yazar.Soyad = model.Soyad;
			Yazar.Sifre = model.Sifre;
			Yazar.Eposta = model.Eposta;
			Yazar.Resim = model.Resim;
			Yazar.Aktifmi = model.Aktifmi;
			var response = _repository.Update(Yazar);

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
