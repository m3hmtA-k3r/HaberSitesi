using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	public class BlogManager : IBlogService
	{
		private readonly IUnitOfWork _unitOfWork;

		public BlogManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<BloglarDto> GetBloglar()
		{
			var response = _unitOfWork.BloglarRepository.GetAll().ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public List<BloglarDto> GetAktifBloglar()
		{
			var response = _unitOfWork.BloglarRepository.GetAll()
				.Where(b => b.AktifMi && b.YayinTarihi <= DateTime.Now)
				.OrderByDescending(b => b.YayinTarihi)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public List<BloglarDto> GetBloglarByKategori(int kategoriId)
		{
			var response = _unitOfWork.BloglarRepository.GetAll()
				.Where(b => b.KategoriId == kategoriId)
				.OrderByDescending(b => b.YayinTarihi)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public List<BloglarDto> GetBloglarByYazar(int yazarId)
		{
			var response = _unitOfWork.BloglarRepository.GetAll()
				.Where(b => b.YazarId == yazarId)
				.OrderByDescending(b => b.YayinTarihi)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public BloglarDto GetBlogById(int id)
		{
			var response = _unitOfWork.BloglarRepository.GetById(id);
			return MapToDto(response);
		}

		public BloglarDto InsertBlog(BloglarDto model)
		{
			var entity = MapToEntity(model);
			entity.OlusturmaTarihi = DateTime.Now;
			if (entity.YayinTarihi == default)
				entity.YayinTarihi = DateTime.Now;

			var response = _unitOfWork.BloglarRepository.Insert(entity);
			_unitOfWork.SaveChanges();

			return MapToDto(response);
		}

		public BloglarDto UpdateBlog(BloglarDto model)
		{
			var blog = _unitOfWork.BloglarRepository.GetById(model.Id);
			if (blog == null)
				return null;

			blog.Baslik = model.Baslik;
			blog.Ozet = model.Ozet;
			blog.Icerik = model.Icerik;
			blog.GorselUrl = model.GorselUrl;
			blog.Etiketler = model.Etiketler;
			blog.YayinTarihi = model.YayinTarihi;
			blog.AktifMi = model.AktifMi;
			blog.KategoriId = model.KategoriId;
			blog.YazarId = model.YazarId;
			blog.GuncellenmeTarihi = DateTime.Now;

			var response = _unitOfWork.BloglarRepository.Update(blog);
			_unitOfWork.SaveChanges();

			return MapToDto(response);
		}

		public bool DeleteBlog(int id)
		{
			var result = _unitOfWork.BloglarRepository.Delete(new Bloglar { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public void IncrementGoruntulenmeSayisi(int id)
		{
			var blog = _unitOfWork.BloglarRepository.GetById(id);
			if (blog != null)
			{
				blog.GoruntulenmeSayisi++;
				_unitOfWork.BloglarRepository.Update(blog);
				_unitOfWork.SaveChanges();
			}
		}

		private BloglarDto MapToDto(Bloglar entity)
		{
			if (entity == null) return null;

			var kategori = entity.KategoriId.HasValue
				? _unitOfWork.BlogKategorilerRepository.GetById(entity.KategoriId.Value)
				: null;

			var yazar = entity.YazarId.HasValue
				? _unitOfWork.KullanicilarRepository.GetById(entity.YazarId.Value)
				: null;

			return new BloglarDto
			{
				Id = entity.Id,
				Baslik = entity.Baslik,
				Ozet = entity.Ozet,
				Icerik = entity.Icerik,
				GorselUrl = entity.GorselUrl,
				Etiketler = entity.Etiketler,
				YayinTarihi = entity.YayinTarihi,
				OlusturmaTarihi = entity.OlusturmaTarihi,
				GuncellenmeTarihi = entity.GuncellenmeTarihi,
				AktifMi = entity.AktifMi,
				KategoriId = entity.KategoriId,
				KategoriAdi = kategori?.Adi,
				YazarId = entity.YazarId,
				YazarAdi = yazar != null ? $"{yazar.Ad} {yazar.Soyad}" : null,
				GoruntulenmeSayisi = entity.GoruntulenmeSayisi
			};
		}

		private Bloglar MapToEntity(BloglarDto dto)
		{
			return new Bloglar
			{
				Id = dto.Id,
				Baslik = dto.Baslik,
				Ozet = dto.Ozet,
				Icerik = dto.Icerik,
				GorselUrl = dto.GorselUrl,
				Etiketler = dto.Etiketler,
				YayinTarihi = dto.YayinTarihi,
				AktifMi = dto.AktifMi,
				KategoriId = dto.KategoriId,
				YazarId = dto.YazarId,
				GoruntulenmeSayisi = dto.GoruntulenmeSayisi
			};
		}
	}
}
