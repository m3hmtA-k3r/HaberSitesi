using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Security;

namespace Business.Base
{
	/// <summary>
	/// Business logic for managing Yazarlar (Authors)
	/// Now uses Unit of Work pattern for better transaction management
	/// Sifre hashleme icin BCrypt kullanilir
	/// </summary>
	public class YazarManager : IYazarService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPasswordHasher _passwordHasher;

		public YazarManager(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
		{
			_unitOfWork = unitOfWork;
			_passwordHasher = passwordHasher;
		}

		/// <summary>
		/// Check if Yazar is linked to unified Kullanici system
		/// </summary>
		public bool IsLinkedToKullanici(int yazarId)
		{
			var yazar = _unitOfWork.YazarlarRepository.GetById(yazarId);
			return yazar?.KullaniciId.HasValue == true && yazar.KullaniciId.Value > 0;
		}

		/// <summary>
		/// Get linked Kullanici ID for a Yazar
		/// </summary>
		public int? GetLinkedKullaniciId(int yazarId)
		{
			var yazar = _unitOfWork.YazarlarRepository.GetById(yazarId);
			return yazar?.KullaniciId;
		}

		public bool DeleteYazar(int id)
		{
			var result = _unitOfWork.YazarlarRepository.Delete(new Yazarlar { Id = id });
			if (result)
			{
				_unitOfWork.SaveChanges();
			}
			return result;
		}

		public YazarlarDto GetYazarByEmailPassword(string email, string password)
		{
			var data = _unitOfWork.YazarlarRepository.GetAll();
			var yazar = data.FirstOrDefault(x => x.Eposta == email);

			if (yazar == null)
				return null;

			bool isValidPassword;

			// If linked to Kullanici system, authenticate via Kullanici
			if (yazar.KullaniciId.HasValue && yazar.KullaniciId.Value > 0)
			{
				var kullanici = _unitOfWork.KullanicilarRepository.GetById(yazar.KullaniciId.Value);
				if (kullanici == null || !kullanici.AktifMi)
					return null;

				isValidPassword = _passwordHasher.VerifyPassword(password, kullanici.SifreHash);

				// Update last login time
				if (isValidPassword)
				{
					kullanici.SonGirisTarihi = DateTime.UtcNow;
					_unitOfWork.KullanicilarRepository.Update(kullanici);
					_unitOfWork.SaveChanges();
				}
			}
			else
			{
				// Legacy authentication - will be deprecated
				// BCrypt ile sifre dogrulama
				if (yazar.Sifre.StartsWith("$2"))
				{
					// BCrypt hash formatinda - guvenli dogrulama
					isValidPassword = _passwordHasher.VerifyPassword(password, yazar.Sifre);
				}
				else
				{
					// Legacy duz metin sifre - gecici uyumluluk (UYARI: Guvenlik riski!)
					isValidPassword = yazar.Sifre == password;

					// Basarili giriste sifreyi otomatik hashle (auto-migration)
					if (isValidPassword)
					{
						yazar.Sifre = _passwordHasher.HashPassword(password);
						_unitOfWork.YazarlarRepository.Update(yazar);
						_unitOfWork.SaveChanges();
					}
				}
			}

			return isValidPassword ? YazarItem(yazar) : null;
		}

		public YazarlarDto GetYazarById(int id)
		{
			var response = _unitOfWork.YazarlarRepository.GetById(id);
			return YazarItem(response);
		}

		public List<YazarlarDto> GetYazarlar()
		{
			var response = _unitOfWork.YazarlarRepository.GetAll().ToList();
			List<YazarlarDto> result = new List<YazarlarDto>();

			foreach (var item in response)
				result.Add(YazarItem(item));

			return result;
		}

		public YazarlarDto InsertYazar(YazarlarDto model)
		{
			// Yeni yazar eklenirken sifre her zaman hashlenir
			var entity = YazarItem(model);
			entity.Sifre = _passwordHasher.HashPassword(model.Sifre);

			var response = _unitOfWork.YazarlarRepository.Insert(entity);
			_unitOfWork.SaveChanges();

			return YazarItem(response);
		}

		public YazarlarDto UpdateYazar(YazarlarDto model)
		{
			var yazar = _unitOfWork.YazarlarRepository.GetById(model.Id);
			yazar.Id = model.Id;
			yazar.Ad = model.Ad;
			yazar.Soyad = model.Soyad;
			yazar.Eposta = model.Eposta;
			yazar.Resim = model.Resim;
			yazar.Aktifmi = model.Aktifmi;

			// Sifre degistiriliyorsa hashle, yoksa mevcut sifreyi koru
			if (!string.IsNullOrEmpty(model.Sifre) && !model.Sifre.StartsWith("$2"))
			{
				yazar.Sifre = _passwordHasher.HashPassword(model.Sifre);
			}
			else if (!string.IsNullOrEmpty(model.Sifre))
			{
				yazar.Sifre = model.Sifre; // Zaten hashli
			}
			// Bos ise mevcut sifre korunur

			var response = _unitOfWork.YazarlarRepository.Update(yazar);
			_unitOfWork.SaveChanges();

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
