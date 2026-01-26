using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	public class BlogKategoriManager : IBlogKategoriService
	{
		private readonly IUnitOfWork _unitOfWork;

		public BlogKategoriManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<BlogKategorilerDto> GetKategoriler()
		{
			var response = _unitOfWork.BlogKategorilerRepository.GetAll()
				.OrderBy(k => k.Sira)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public List<BlogKategorilerDto> GetAktifKategoriler()
		{
			var response = _unitOfWork.BlogKategorilerRepository.GetAll()
				.Where(k => k.AktifMi)
				.OrderBy(k => k.Sira)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public BlogKategorilerDto GetKategoriById(int id)
		{
			var response = _unitOfWork.BlogKategorilerRepository.GetById(id);
			return MapToDto(response);
		}

		public BlogKategorilerDto InsertKategori(BlogKategorilerDto model)
		{
			var entity = MapToEntity(model);
			entity.OlusturmaTarihi = DateTime.Now;

			var response = _unitOfWork.BlogKategorilerRepository.Insert(entity);
			_unitOfWork.SaveChanges();

			return MapToDto(response);
		}

		public BlogKategorilerDto UpdateKategori(BlogKategorilerDto model)
		{
			var kategori = _unitOfWork.BlogKategorilerRepository.GetById(model.Id);
			if (kategori == null)
				return null;

			kategori.Adi = model.Adi;
			kategori.Aciklama = model.Aciklama;
			kategori.Sira = model.Sira;
			kategori.AktifMi = model.AktifMi;

			var response = _unitOfWork.BlogKategorilerRepository.Update(kategori);
			_unitOfWork.SaveChanges();

			return MapToDto(response);
		}

		public bool DeleteKategori(int id)
		{
			var result = _unitOfWork.BlogKategorilerRepository.Delete(new BlogKategoriler { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		private BlogKategorilerDto MapToDto(BlogKategoriler entity)
		{
			if (entity == null) return null;

			var blogSayisi = _unitOfWork.BloglarRepository.GetAll()
				.Count(b => b.KategoriId == entity.Id);

			return new BlogKategorilerDto
			{
				Id = entity.Id,
				Adi = entity.Adi,
				Aciklama = entity.Aciklama,
				Sira = entity.Sira,
				AktifMi = entity.AktifMi,
				OlusturmaTarihi = entity.OlusturmaTarihi,
				BlogSayisi = blogSayisi
			};
		}

		private BlogKategoriler MapToEntity(BlogKategorilerDto dto)
		{
			return new BlogKategoriler
			{
				Id = dto.Id,
				Adi = dto.Adi,
				Aciklama = dto.Aciklama,
				Sira = dto.Sira,
				AktifMi = dto.AktifMi
			};
		}
	}
}
