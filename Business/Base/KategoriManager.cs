using Business.Abstract;
using DataAccess.Abstract.Repository;
using Shared.Dtos;
using Shared.Entities;

namespace Business.Base
{
	public class KategoriManager : IKategoriService
	{
		private readonly IRepository<Kategoriler> _repository;
		public KategoriManager(IRepository<Kategoriler> repository)
		{
			_repository = repository;
		}
		public bool DeleteKategori(int id)
		{
			return _repository.Delete(new Kategoriler { Id = id });
		}

		public KategorilerDto GetKategoriById(int id)
		{
			var response = _repository.GetById(id);

			return KategoriItem(response);
		}

		public List<KategorilerDto> GetKategoriler()
		{
			var response = _repository.GetAll().ToList();

			List<KategorilerDto> result = new List<KategorilerDto>();

			foreach (var item in response)
				result.Add(KategoriItem(item));

			return result;
		}

		public KategorilerDto InsertKategori(KategorilerDto model)
		{
			Kategoriler response = _repository.Insert(KategoriItem(model));

			return KategoriItem(response);
		}

		public KategorilerDto UpdateKategori(KategorilerDto model)
		{
			var kategori = _repository.GetById(model.Id);
			kategori.Aktifmi = model.Aktifmi;
			kategori.Aciklama = model.Aciklama;
			Kategoriler response = _repository.Update(kategori);

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
