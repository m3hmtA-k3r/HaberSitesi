using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	public class SistemLogManager : ISistemLogService
	{
		private readonly IUnitOfWork _unitOfWork;

		public SistemLogManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<SistemLogDto> GetLoglar()
		{
			var response = _unitOfWork.SistemLoglarRepository
				.GetAll()
				.OrderByDescending(l => l.Tarih)
				.ToList();

			return response.Select(LogToDto).ToList();
		}

		public List<SistemLogDto> GetLoglarBySeviye(string seviye)
		{
			var response = _unitOfWork.SistemLoglarRepository
				.GetAll()
				.Where(l => l.Seviye.ToLower() == seviye.ToLower())
				.OrderByDescending(l => l.Tarih)
				.ToList();

			return response.Select(LogToDto).ToList();
		}

		public List<SistemLogDto> GetLoglarByModul(string modul)
		{
			var response = _unitOfWork.SistemLoglarRepository
				.GetAll()
				.Where(l => l.Modul.ToLower() == modul.ToLower())
				.OrderByDescending(l => l.Tarih)
				.ToList();

			return response.Select(LogToDto).ToList();
		}

		public SistemLogDto GetLogById(int id)
		{
			var response = _unitOfWork.SistemLoglarRepository.GetById(id);
			return response != null ? LogToDto(response) : null!;
		}

		public SistemLogDto InsertLog(SistemLogDto model)
		{
			var entity = DtoToLog(model);
			entity.Tarih = DateTime.UtcNow;

			var response = _unitOfWork.SistemLoglarRepository.Insert(entity);
			_unitOfWork.SaveChanges();

			return LogToDto(response);
		}

		public bool DeleteLog(int id)
		{
			var entity = _unitOfWork.SistemLoglarRepository.GetById(id);
			if (entity == null) return false;

			var result = _unitOfWork.SistemLoglarRepository.Delete(entity);
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public bool DeleteAllLogs()
		{
			var allLogs = _unitOfWork.SistemLoglarRepository.GetAll().ToList();
			foreach (var log in allLogs)
			{
				_unitOfWork.SistemLoglarRepository.Delete(log);
			}
			_unitOfWork.SaveChanges();
			return true;
		}

		private static SistemLogDto LogToDto(SistemLog entity)
		{
			return new SistemLogDto
			{
				Id = entity.Id,
				KullaniciId = entity.KullaniciId,
				KullaniciAdi = entity.KullaniciAdi,
				IslemTipi = entity.IslemTipi,
				Modul = entity.Modul,
				Aciklama = entity.Aciklama,
				IpAdresi = entity.IpAdresi,
				Tarih = entity.Tarih,
				Seviye = entity.Seviye
			};
		}

		private static SistemLog DtoToLog(SistemLogDto dto)
		{
			return new SistemLog
			{
				Id = dto.Id,
				KullaniciId = dto.KullaniciId,
				KullaniciAdi = dto.KullaniciAdi,
				IslemTipi = dto.IslemTipi,
				Modul = dto.Modul,
				Aciklama = dto.Aciklama,
				IpAdresi = dto.IpAdresi,
				Tarih = dto.Tarih,
				Seviye = dto.Seviye
			};
		}
	}
}
