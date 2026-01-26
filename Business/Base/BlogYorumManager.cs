using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	public class BlogYorumManager : IBlogYorumService
	{
		private readonly IUnitOfWork _unitOfWork;

		public BlogYorumManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<BlogYorumlarDto> GetYorumlar()
		{
			var response = _unitOfWork.BlogYorumlarRepository.GetAll()
				.OrderByDescending(y => y.OlusturmaTarihi)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public List<BlogYorumlarDto> GetYorumlarByBlog(int blogId)
		{
			var response = _unitOfWork.BlogYorumlarRepository.GetAll()
				.Where(y => y.BlogId == blogId)
				.OrderByDescending(y => y.OlusturmaTarihi)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public List<BlogYorumlarDto> GetOnayliYorumlarByBlog(int blogId)
		{
			var response = _unitOfWork.BlogYorumlarRepository.GetAll()
				.Where(y => y.BlogId == blogId && y.OnaylandiMi && y.AktifMi)
				.OrderByDescending(y => y.OlusturmaTarihi)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public List<BlogYorumlarDto> GetOnayBekleyenYorumlar()
		{
			var response = _unitOfWork.BlogYorumlarRepository.GetAll()
				.Where(y => !y.OnaylandiMi && y.AktifMi)
				.OrderByDescending(y => y.OlusturmaTarihi)
				.ToList();
			return response.Select(item => MapToDto(item)).ToList();
		}

		public BlogYorumlarDto GetYorumById(int id)
		{
			var response = _unitOfWork.BlogYorumlarRepository.GetById(id);
			return MapToDto(response);
		}

		public BlogYorumlarDto InsertYorum(BlogYorumlarDto model)
		{
			var entity = MapToEntity(model);
			entity.OlusturmaTarihi = DateTime.Now;
			entity.OnaylandiMi = false;
			entity.AktifMi = true;

			var response = _unitOfWork.BlogYorumlarRepository.Insert(entity);
			_unitOfWork.SaveChanges();

			return MapToDto(response);
		}

		public BlogYorumlarDto UpdateYorum(BlogYorumlarDto model)
		{
			var yorum = _unitOfWork.BlogYorumlarRepository.GetById(model.Id);
			if (yorum == null)
				return null;

			yorum.Ad = model.Ad;
			yorum.Soyad = model.Soyad;
			yorum.Eposta = model.Eposta;
			yorum.Yorum = model.Yorum;
			yorum.OnaylandiMi = model.OnaylandiMi;
			yorum.AktifMi = model.AktifMi;

			var response = _unitOfWork.BlogYorumlarRepository.Update(yorum);
			_unitOfWork.SaveChanges();

			return MapToDto(response);
		}

		public bool DeleteYorum(int id)
		{
			var result = _unitOfWork.BlogYorumlarRepository.Delete(new BlogYorumlar { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public bool OnaylaYorum(int id)
		{
			var yorum = _unitOfWork.BlogYorumlarRepository.GetById(id);
			if (yorum == null)
				return false;

			yorum.OnaylandiMi = true;
			_unitOfWork.BlogYorumlarRepository.Update(yorum);
			_unitOfWork.SaveChanges();
			return true;
		}

		public bool ReddetYorum(int id)
		{
			var yorum = _unitOfWork.BlogYorumlarRepository.GetById(id);
			if (yorum == null)
				return false;

			yorum.AktifMi = false;
			_unitOfWork.BlogYorumlarRepository.Update(yorum);
			_unitOfWork.SaveChanges();
			return true;
		}

		private BlogYorumlarDto MapToDto(BlogYorumlar entity)
		{
			if (entity == null) return null;

			var blog = _unitOfWork.BloglarRepository.GetById(entity.BlogId);

			return new BlogYorumlarDto
			{
				Id = entity.Id,
				BlogId = entity.BlogId,
				BlogBaslik = blog?.Baslik,
				Ad = entity.Ad,
				Soyad = entity.Soyad,
				Eposta = entity.Eposta,
				Yorum = entity.Yorum,
				OnaylandiMi = entity.OnaylandiMi,
				AktifMi = entity.AktifMi,
				OlusturmaTarihi = entity.OlusturmaTarihi
			};
		}

		private BlogYorumlar MapToEntity(BlogYorumlarDto dto)
		{
			return new BlogYorumlar
			{
				Id = dto.Id,
				BlogId = dto.BlogId,
				Ad = dto.Ad,
				Soyad = dto.Soyad,
				Eposta = dto.Eposta,
				Yorum = dto.Yorum,
				OnaylandiMi = dto.OnaylandiMi,
				AktifMi = dto.AktifMi
			};
		}
	}
}
