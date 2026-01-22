using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
	/// <summary>
	/// Business logic for managing Slaytlar (Sliders)
	/// Now uses Unit of Work pattern for better transaction management
	/// </summary>
	public class SlaytManager : ISlaytService
	{
		private readonly IUnitOfWork _unitOfWork;

		public SlaytManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public int GetUnpublishedSlidesCount()
		{
			return _unitOfWork.SlaytlarRepository.GetAll().Count(x => x.Aktifmi == false);
		}

		public bool DeleteSlayt(int id)
		{
			var result = _unitOfWork.SlaytlarRepository.Delete(new Slaytlar { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public SlaytlarDto GetSlaytById(int id)
		{
			var response = _unitOfWork.SlaytlarRepository.GetById(id);
			return SlaytItem(response);
		}

		public List<SlaytlarDto> GetSlaytlar()
		{
			var response = _unitOfWork.SlaytlarRepository.GetAll().ToList();
			List<SlaytlarDto> result = new List<SlaytlarDto>();

			foreach (var item in response)
				result.Add(SlaytItem(item));

			return result;
		}

		public SlaytlarDto InsertSlayt(SlaytlarDto model)
		{
			var response = _unitOfWork.SlaytlarRepository.Insert(SlaytItem(model));
			_unitOfWork.SaveChanges();

			return SlaytItem(response);
		}

		public SlaytlarDto UpdateSlayt(SlaytlarDto model)
		{
			var slayt = _unitOfWork.SlaytlarRepository.GetById(model.Id);
			slayt.Aktifmi = model.Aktifmi;
			slayt.Resim = model.Resim;
			slayt.Icerik = model.Icerik;
			slayt.Baslik = model.Baslik;
			slayt.HaberId = model.HaberId;
			var response = _unitOfWork.SlaytlarRepository.Update(slayt);
			_unitOfWork.SaveChanges();

			return SlaytItem(response);
		}

		private SlaytlarDto SlaytItem(Slaytlar model)
		{
			SlaytlarDto result = new SlaytlarDto();
			result.Id = model.Id;
			result.Baslik = model.Baslik;
			result.Icerik = model.Icerik;
			result.HaberId = model.HaberId;

			var haber = _unitOfWork.HaberlerRepository.GetById(model.HaberId);
			result.Haber = haber?.Baslik ?? "Haber Bulunamadı";
			result.Resim = model.Resim;
			result.Aktifmi = model.Aktifmi;
			return result;
		}
		private Slaytlar SlaytItem(SlaytlarDto model)
		{
			Slaytlar result = new Slaytlar();
			result.Id = model.Id;
			result.Baslik = model.Baslik;
			result.Icerik = model.Icerik;
			result.HaberId = model.HaberId;
			result.Resim = model.Resim;
			result.Aktifmi = model.Aktifmi;
			return result;
		}
	}
}
