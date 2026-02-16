using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Domain.Entities;
using Infrastructure.Security;

namespace Business.Base
{
	/// <summary>
	/// Service for migrating legacy Yazarlar to the unified Kullanicilar system
	/// Links existing Yazarlar to Kullanici accounts with "Yazar" role
	/// </summary>
	public interface IYazarMigrationService
	{
		/// <summary>
		/// Migrate all unlinked Yazarlar to Kullanici accounts
		/// Returns the number of successfully migrated authors
		/// </summary>
		Task<int> MigrateAllYazarlarAsync();

		/// <summary>
		/// Migrate a specific Yazar to a Kullanici account
		/// </summary>
		Task<bool> MigrateYazarAsync(int yazarId);

		/// <summary>
		/// Get migration status - how many Yazarlar are linked vs unlinked
		/// </summary>
		(int Total, int Linked, int Unlinked) GetMigrationStatus();
	}

	public class YazarMigrationService : IYazarMigrationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPasswordHasher _passwordHasher;
		private const string YazarRolAdi = "Yazar";

		public YazarMigrationService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
		{
			_unitOfWork = unitOfWork;
			_passwordHasher = passwordHasher;
		}

		public async Task<int> MigrateAllYazarlarAsync()
		{
			// Ensure Yazar role exists
			var yazarRol = await EnsureYazarRolExistsAsync();

			// Get all unlinked Yazarlar
			var yazarlar = _unitOfWork.YazarlarRepository.GetAll()
				.Where(y => y.KullaniciId == null || y.KullaniciId == 0)
				.ToList();

			int migratedCount = 0;

			foreach (var yazar in yazarlar)
			{
				try
				{
					if (await MigrateYazarInternalAsync(yazar, yazarRol.Id))
					{
						migratedCount++;
					}
				}
				catch
				{
					// Log error and continue with next Yazar
					continue;
				}
			}

			return migratedCount;
		}

		public async Task<bool> MigrateYazarAsync(int yazarId)
		{
			var yazar = _unitOfWork.YazarlarRepository.GetById(yazarId);
			if (yazar == null)
				return false;

			// Already linked
			if (yazar.KullaniciId.HasValue && yazar.KullaniciId.Value > 0)
				return true;

			var yazarRol = await EnsureYazarRolExistsAsync();
			return await MigrateYazarInternalAsync(yazar, yazarRol.Id);
		}

		public (int Total, int Linked, int Unlinked) GetMigrationStatus()
		{
			var allYazarlar = _unitOfWork.YazarlarRepository.GetAll().ToList();
			var total = allYazarlar.Count;
			var linked = allYazarlar.Count(y => y.KullaniciId.HasValue && y.KullaniciId.Value > 0);
			var unlinked = total - linked;

			return (total, linked, unlinked);
		}

		private async Task<Roller> EnsureYazarRolExistsAsync()
		{
			var existingRol = _unitOfWork.RollerRepository.GetAll()
				.FirstOrDefault(r => r.RolAdi == YazarRolAdi);

			if (existingRol != null)
				return existingRol;

			// Create Yazar role
			var yazarRol = new Roller
			{
				RolAdi = YazarRolAdi,
				Aciklama = "Yazar - Icerik olusturma ve duzenleme yetkisi",
				AktifMi = true
			};

			var result = _unitOfWork.RollerRepository.Insert(yazarRol);
			await Task.Run(() => _unitOfWork.SaveChanges());

			return result;
		}

		private async Task<bool> MigrateYazarInternalAsync(Yazarlar yazar, int yazarRolId)
		{
			// Check if a Kullanici with same email already exists
			var existingKullanici = _unitOfWork.KullanicilarRepository.GetAll()
				.FirstOrDefault(k => k.Eposta.ToLower() == yazar.Eposta.ToLower());

			int kullaniciId;

			if (existingKullanici != null)
			{
				// Link to existing Kullanici
				kullaniciId = existingKullanici.Id;
			}
			else
			{
				// Create new Kullanici from Yazar data
				var sifreHash = yazar.Sifre.StartsWith("$2")
					? yazar.Sifre // Already BCrypt hashed
					: _passwordHasher.HashPassword(yazar.Sifre); // Hash legacy password

				var yeniKullanici = new Kullanicilar
				{
					Ad = yazar.Ad,
					Soyad = yazar.Soyad,
					Eposta = yazar.Eposta,
					SifreHash = sifreHash,
					Resim = string.IsNullOrEmpty(yazar.Resim) ? null : yazar.Resim,
					AktifMi = yazar.Aktifmi,
					OlusturmaTarihi = DateTime.UtcNow
				};

				var insertedKullanici = _unitOfWork.KullanicilarRepository.Insert(yeniKullanici);
				_unitOfWork.SaveChanges();
				kullaniciId = insertedKullanici.Id;
			}

			// Assign Yazar role if not already assigned
			var existingRol = _unitOfWork.KullaniciRollerRepository.GetAll()
				.FirstOrDefault(kr => kr.KullaniciId == kullaniciId && kr.RolId == yazarRolId);

			if (existingRol == null)
			{
				var kullaniciRol = new KullaniciRol
				{
					KullaniciId = kullaniciId,
					RolId = yazarRolId,
					AtanmaTarihi = DateTime.UtcNow
				};
				_unitOfWork.KullaniciRollerRepository.Insert(kullaniciRol);
			}

			// Link Yazar to Kullanici
			yazar.KullaniciId = kullaniciId;
			_unitOfWork.YazarlarRepository.Update(yazar);

			await Task.Run(() => _unitOfWork.SaveChanges());

			return true;
		}
	}
}
