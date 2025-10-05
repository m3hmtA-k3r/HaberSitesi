using Business.Abstract;
using DataAccess.Abstract.Repository;
using Shared.Dtos;
using Shared.Entities;

namespace Business.Base
{
	public class YorumManager : IYorumService
	{
		private readonly IRepository<Yorumlar> _repository;
		private readonly IRepository<Haberler> _haberRepository;
		public YorumManager(IRepository<Yorumlar> repository, IRepository<Haberler> haberRepository)
		{
			_repository = repository;
			_haberRepository = haberRepository;
		}

		public int GetOnayBekleyenYorumSayisi()
		{
            return _repository.GetAll().Count(yorum => yorum.Aktifmi == false);
        }

        public bool DeleteYorum(int id)
		{
			return _repository.Delete(new Yorumlar { Id = id });
		}

		public YorumlarDto GetYorumById(int id)
		{
			var response = _repository.GetById(id);

			return YorumItem(response);
		}

		public List<YorumlarDto> GetYorumlar()
		{
			var response = _repository.GetAll().ToList();
			List<YorumlarDto> result = new List<YorumlarDto>();

			foreach (var item in response)
				result.Add(YorumItem(item));

			return result;
		}

		public YorumlarDto InsertYorum(YorumlarDto model)
		{
			model.EklenmeTarihi = DateTime.Now;
			var response = _repository.Insert(YorumItem(model));

			return YorumItem(response);
		}

		public YorumlarDto UpdateYorum(YorumlarDto model)
		{
			var yorum = _repository.GetById(model.Id);
			yorum.Id = model.Id;
			yorum.Ad = model.Ad;
			yorum.Soyad = model.Soyad;
			yorum.HaberId = model.HaberId;
			yorum.Eposta = model.Eposta;
			yorum.Baslik = model.Baslik;
			yorum.Icerik = model.Icerik;
			yorum.Aktifmi = model.Aktifmi;
			var response = _repository.Update(yorum);

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
			result.HaberBaslik = _haberRepository.GetById(model.HaberId).Baslik;
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
