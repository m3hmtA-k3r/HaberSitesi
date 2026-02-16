using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	/// <summary>
	/// Business logic for managing contact form messages
	/// </summary>
	public class IletisimManager : IIletisimService
	{
		private readonly IUnitOfWork _unitOfWork;

		public IletisimManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<IletisimMesajlariDto> GetMesajlar()
		{
			var response = _unitOfWork.IletisimMesajlariRepository
				.GetAll()
				.OrderByDescending(m => m.EklemeTarihi)
				.ToList();

			return response.Select(MesajToDto).ToList();
		}

		public List<IletisimMesajlariDto> GetOkunmamisMesajlar()
		{
			var response = _unitOfWork.IletisimMesajlariRepository
				.GetAll()
				.Where(m => !m.OkunduMu)
				.OrderByDescending(m => m.EklemeTarihi)
				.ToList();

			return response.Select(MesajToDto).ToList();
		}

		public IletisimMesajlariDto GetMesajById(int id)
		{
			var response = _unitOfWork.IletisimMesajlariRepository.GetById(id);
			return response != null ? MesajToDto(response) : null!;
		}

		public IletisimMesajlariDto InsertMesaj(IletisimMesajlariDto model)
		{
			var entity = DtoToMesaj(model);
			entity.EklemeTarihi = DateTime.UtcNow;
			entity.OkunduMu = false;
			entity.CevaplandiMi = false;

			var response = _unitOfWork.IletisimMesajlariRepository.Insert(entity);
			_unitOfWork.SaveChanges();

			return MesajToDto(response);
		}

		public bool OkunduOlarakIsaretle(int id)
		{
			var mesaj = _unitOfWork.IletisimMesajlariRepository.GetById(id);
			if (mesaj == null) return false;

			mesaj.OkunduMu = true;
			_unitOfWork.IletisimMesajlariRepository.Update(mesaj);
			_unitOfWork.SaveChanges();

			return true;
		}

		public bool CevaplandiOlarakIsaretle(int id)
		{
			var mesaj = _unitOfWork.IletisimMesajlariRepository.GetById(id);
			if (mesaj == null) return false;

			mesaj.CevaplandiMi = true;
			mesaj.CevapTarihi = DateTime.UtcNow;
			_unitOfWork.IletisimMesajlariRepository.Update(mesaj);
			_unitOfWork.SaveChanges();

			return true;
		}

		public bool DeleteMesaj(int id)
		{
			var result = _unitOfWork.IletisimMesajlariRepository.Delete(new IletisimMesajlari { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public int GetOkunmamisMesajSayisi()
		{
			return _unitOfWork.IletisimMesajlariRepository
				.GetAll()
				.Count(m => !m.OkunduMu);
		}

		private static IletisimMesajlariDto MesajToDto(IletisimMesajlari entity)
		{
			return new IletisimMesajlariDto
			{
				Id = entity.Id,
				Ad = entity.Ad,
				Eposta = entity.Eposta,
				Konu = entity.Konu,
				Mesaj = entity.Mesaj,
				IpAdresi = entity.IpAdresi,
				EklemeTarihi = entity.EklemeTarihi,
				OkunduMu = entity.OkunduMu,
				CevaplandiMi = entity.CevaplandiMi,
				CevapTarihi = entity.CevapTarihi
			};
		}

		private static IletisimMesajlari DtoToMesaj(IletisimMesajlariDto dto)
		{
			return new IletisimMesajlari
			{
				Id = dto.Id,
				Ad = dto.Ad,
				Eposta = dto.Eposta,
				Konu = dto.Konu,
				Mesaj = dto.Mesaj,
				IpAdresi = dto.IpAdresi,
				EklemeTarihi = dto.EklemeTarihi,
				OkunduMu = dto.OkunduMu,
				CevaplandiMi = dto.CevaplandiMi,
				CevapTarihi = dto.CevapTarihi
			};
		}
	}
}
