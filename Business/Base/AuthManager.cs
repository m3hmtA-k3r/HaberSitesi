using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Security;
using Infrastructure.Identity;

namespace Business.Base
{
    public class AuthManager : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthManager(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public (KullaniciDto? Kullanici, string? Token, string? Hata) Login(string eposta, string sifre)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetAll()
                .FirstOrDefault(k => k.Eposta.ToLower() == eposta.ToLower());

            if (kullanici == null)
            {
                return (null, null, "Kullanici bulunamadi");
            }

            if (!kullanici.AktifMi)
            {
                return (null, null, "Kullanici hesabi aktif degil");
            }

            if (!_passwordHasher.VerifyPassword(sifre, kullanici.SifreHash))
            {
                return (null, null, "Sifre hatali");
            }

            // Update last login time
            kullanici.SonGirisTarihi = DateTime.UtcNow;
            _unitOfWork.KullanicilarRepository.Update(kullanici);
            _unitOfWork.SaveChanges();

            // Get user roles
            var roller = GetKullaniciRolleri(kullanici.Id);

            // Generate token with roles
            var fullName = $"{kullanici.Ad} {kullanici.Soyad}";
            var token = _jwtTokenService.GenerateToken(kullanici.Id, kullanici.Eposta, fullName, roller);

            var kullaniciDto = MapToDto(kullanici, roller);

            return (kullaniciDto, token, null);
        }

        public ProfilDto? GetProfil(int kullaniciId)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetById(kullaniciId);
            if (kullanici == null) return null;

            var roller = GetKullaniciRolleri(kullaniciId);

            return new ProfilDto
            {
                Id = kullanici.Id,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Eposta = kullanici.Eposta,
                Resim = kullanici.Resim,
                OlusturmaTarihi = kullanici.OlusturmaTarihi,
                SonGirisTarihi = kullanici.SonGirisTarihi,
                Roller = roller
            };
        }

        public ProfilDto? UpdateProfil(int kullaniciId, ProfilGuncelleDto model)
        {
            var kullanici = _unitOfWork.KullanicilarRepository.GetById(kullaniciId);
            if (kullanici == null) return null;

            kullanici.Ad = model.Ad;
            kullanici.Soyad = model.Soyad;
            kullanici.Resim = model.Resim;

            _unitOfWork.KullanicilarRepository.Update(kullanici);
            _unitOfWork.SaveChanges();

            return GetProfil(kullaniciId);
        }

        public bool SifreDegistir(int kullaniciId, SifreDegistirDto model)
        {
            if (model.YeniSifre != model.YeniSifreTekrar)
            {
                return false;
            }

            var kullanici = _unitOfWork.KullanicilarRepository.GetById(kullaniciId);
            if (kullanici == null) return false;

            if (!_passwordHasher.VerifyPassword(model.EskiSifre, kullanici.SifreHash))
            {
                return false;
            }

            kullanici.SifreHash = _passwordHasher.HashPassword(model.YeniSifre);
            _unitOfWork.KullanicilarRepository.Update(kullanici);
            _unitOfWork.SaveChanges();

            return true;
        }

        private List<string> GetKullaniciRolleri(int kullaniciId)
        {
            var kullaniciRoller = _unitOfWork.KullaniciRollerRepository.GetAll()
                .Where(kr => kr.KullaniciId == kullaniciId)
                .Select(kr => kr.RolId)
                .ToList();

            var roller = _unitOfWork.RollerRepository.GetAll()
                .Where(r => kullaniciRoller.Contains(r.Id))
                .Select(r => r.RolAdi)
                .ToList();

            return roller;
        }

        private KullaniciDto MapToDto(Kullanicilar kullanici, List<string> roller)
        {
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
                Roller = roller
            };
        }
    }
}
