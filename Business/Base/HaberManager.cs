using Business.Abstract;
using DataAccess.Abstract.Repository;
using Shared.Dtos;
using Shared.Entities;

namespace Business.Base
{
	public class HaberManager : IHaberService
	{
		private readonly IRepository<Haberler> _repository;
		private readonly IRepository<Yazarlar> _repositoryYazar;
		private readonly IRepository<Kategoriler> _repositoryKategori;
		public HaberManager(IRepository<Haberler> repository, IRepository<Yazarlar> repositoryYazar, IRepository<Kategoriler> repositoryKategori)
		{
			_repository = repository;
			_repositoryYazar = repositoryYazar;
			_repositoryKategori = repositoryKategori;
		}
		public bool DeleteHaber(int id)
		{
			return _repository.Delete(new Haberler { Id = id});
		}

		public HaberlerDto GetHaberById(int id)
		{
			var response = _repository.GetById(id);

			return HaberItem(response);
		}

		public List<HaberlerDto> GetHaberler()
		{
			var response = _repository.GetAll().ToList();

			List<HaberlerDto> result = new List<HaberlerDto>();

			foreach (var item in response) 
				result.Add(HaberItem(item));

			return result;
		}

		public HaberlerDto InsertHaber(HaberlerDto model)
		{
			model.EklenmeTarihi = DateTime.Now;
			Haberler response = _repository.Insert(HaberItem(model));

			return HaberItem(response);
		}

		public HaberlerDto UpdateHaber(HaberlerDto model)
		{
			var haber = _repository.GetById(model.Id);
			haber.Id = model.Id;
			haber.Baslik = model.Baslik;
			haber.Icerik = model.Icerik;
			haber.Aktifmi = model.Aktifmi;
			haber.Resim = model.Resim;
			haber.YazarId = model.YazarId;
			haber.KategoriId = model.KategoriId;
			haber.Video = model.Video;

			Haberler response = _repository.Update(haber);

			return HaberItem(response);
		}

		private HaberlerDto HaberItem(Haberler model)
		{ 
			var yazar = _repositoryYazar.GetById(model.YazarId);

            HaberlerDto result = new HaberlerDto();
			result.Id = model.Id;
			result.Baslik = model.Baslik;
			result.Icerik = model.Icerik;
			result.Aktifmi = model.Aktifmi;
			result.Resim = model.Resim;
			result.EklenmeTarihi = model.EklenmeTarihi;
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
			result.Kategori = _repositoryKategori.GetById(model.KategoriId).Aciklama;
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
			result.EklenmeTarihi = model.EklenmeTarihi;
			result.YazarId = model.YazarId;
			result.KategoriId = model.KategoriId;
			result.GosterimSayisi = model.GosterimSayisi;
			result.Video = model.Video;
			return result;
		}
	}
}
