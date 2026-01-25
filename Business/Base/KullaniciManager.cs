using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Security;

namespace Business.Base
{
    public class KullaniciManager : IKullaniciService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public KullaniciManager(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public List<KullaniciDto> GetKullanicilar()
        {
            var kullanicilar = _unitOfWork.KullanicilarRepository.GetAll().ToList();
            var result = new List<KullaniciDto>();

            foreach (var kullanici in kullanicilar)
            {
                result.Add(MapToDto(kullanici));
            }

            return result;
        }

        public KullaniciDto? GetKullaniciById(int id)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetById(id);
            if (kullanici == null) return null;

            return MapToDto(kullanici);
        }

        public KullaniciDto? GetKullaniciByEposta(string eposta)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetAll()
                .FirstOrDefault(k => k.Eposta.ToLower() == eposta.ToLower());

            if (kullanici == null) return null;

            return MapToDto(kullanici);
        }

        public KullaniciDto CreateKullanici(KullaniciCreateDto model)
        {
            var kullanici = new Kullanicilar
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Eposta = model.Eposta,
                SifreHash = _passwordHasher.HashPassword(model.Sifre),
                Resim = model.Resim,
                AktifMi = model.AktifMi,
                OlusturmaTarihi = DateTime.UtcNow
            };

            var result = _unitOfWork.KullanicilarRepository.Insert(kullanici);
            _unitOfWork.SaveChanges();

            // Assign roles if provided
            if (model.RolIdler != null && model.RolIdler.Any())
            {
                foreach (var rolId in model.RolIdler)
                {
                    var kullaniciRol = new KullaniciRol
                    {
                        KullaniciId = result.Id,
                        RolId = rolId,
                        AtanmaTarihi = DateTime.UtcNow
                    };
                    _unitOfWork.KullaniciRollerRepository.Insert(kullaniciRol);
                }
                _unitOfWork.SaveChanges();
            }

            return MapToDto(result);
        }

        public KullaniciDto? UpdateKullanici(KullaniciUpdateDto model)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetById(model.Id);
            if (kullanici == null) return null;

            kullanici.Ad = model.Ad;
            kullanici.Soyad = model.Soyad;
            kullanici.Eposta = model.Eposta;
            kullanici.Resim = model.Resim;
            kullanici.AktifMi = model.AktifMi;

            if (!string.IsNullOrEmpty(model.Sifre))
            {
                kullanici.SifreHash = _passwordHasher.HashPassword(model.Sifre);
            }

            var result = _unitOfWork.KullanicilarRepository.Update(kullanici);
            _unitOfWork.SaveChanges();

            return MapToDto(result);
        }

        public bool DeleteKullanici(int id)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetById(id);
            if (kullanici == null) return false;

            // Delete user roles first
            var kullaniciRoller = _unitOfWork.KullaniciRollerRepository.GetAll()
                .Where(kr => kr.KullaniciId == id).ToList();

            foreach (var rol in kullaniciRoller)
            {
                _unitOfWork.KullaniciRollerRepository.Delete(rol);
            }

            var result = _unitOfWork.KullanicilarRepository.Delete(kullanici);
            if (result)
            {
                _unitOfWork.SaveChanges();
            }

            return result;
        }

        public bool ValidateCredentials(string eposta, string sifre)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetAll()
                .FirstOrDefault(k => k.Eposta.ToLower() == eposta.ToLower() && k.AktifMi);

            if (kullanici == null) return false;

            return _passwordHasher.VerifyPassword(sifre, kullanici.SifreHash);
        }

        public bool SifreDegistir(int kullaniciId, string eskiSifre, string yeniSifre)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetById(kullaniciId);
            if (kullanici == null) return false;

            if (!_passwordHasher.VerifyPassword(eskiSifre, kullanici.SifreHash))
            {
                return false;
            }

            kullanici.SifreHash = _passwordHasher.HashPassword(yeniSifre);
            _unitOfWork.KullanicilarRepository.Update(kullanici);
            _unitOfWork.SaveChanges();

            return true;
        }

        private KullaniciDto MapToDto(Kullanicilar kullanici)
        {
            var roller = _unitOfWork.KullaniciRollerRepository.GetAll()
                .Where(kr => kr.KullaniciId == kullanici.Id)
                .Select(kr => kr.RolId)
                .ToList();

            var rolAdlari = _unitOfWork.RollerRepository.GetAll()
                .Where(r => roller.Contains(r.Id))
                .Select(r => r.RolAdi)
                .ToList();

            return new KullaniciDto
            {
                Id = kullanici.Id,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Eposta = kullanici.Eposta,
                Resim = kullanici.Resim,
                AktifMi = kullanici.AktifMi,
                OlusturmaTarihi = kullanici.OlusturmaTarihi,
                SonGirisTarihi = kullanici.SonGirisTarihi,
                Roller = rolAdlari
            };
        }
    }
}
