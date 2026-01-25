using Business.Abstract;
using DataAccess.Abstract.UnitOfWork;
using Application.DTOs;
using Domain.Entities;

namespace Business.Base
{
    public class RolManager : IRolService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<RolDto> GetRoller()
        {
            var roller = _unitOfWork.RollerRepository.GetAll().ToList();
            return roller.Select(MapToDto).ToList();
        }

        public RolDto? GetRolById(int id)
        {
            var rol = _unitOfWork.RollerRepository.GetById(id);
            if (rol == null) return null;

            return MapToDto(rol);
        }

        public List<RolDto> GetKullaniciRolleri(int kullaniciId)
        {
            var kullaniciRoller = _unitOfWork.KullaniciRollerRepository.GetAll()
                .Where(kr => kr.KullaniciId == kullaniciId)
                .Select(kr => kr.RolId)
                .ToList();

            var roller = _unitOfWork.RollerRepository.GetAll()
                .Where(r => kullaniciRoller.Contains(r.Id))
                .ToList();

            return roller.Select(MapToDto).ToList();
        }

        public bool AtaRol(int kullaniciId, int rolId)
        {
            // Check if user exists
            var kullanici = _unitOfWork.KullanicilarRepository.GetById(kullaniciId);
            if (kullanici == null) return false;

            // Check if role exists
            var rol = _unitOfWork.RollerRepository.GetById(rolId);
            if (rol == null) return false;

            // Check if already assigned
            var exists = _unitOfWork.KullaniciRollerRepository.GetAll()
                .Any(kr => kr.KullaniciId == kullaniciId && kr.RolId == rolId);

            if (exists) return true; // Already assigned

            var kullaniciRol = new KullaniciRol
            {
                KullaniciId = kullaniciId,
                RolId = rolId,
                AtanmaTarihi = DateTime.UtcNow
            };

            _unitOfWork.KullaniciRollerRepository.Insert(kullaniciRol);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool KaldirRol(int kullaniciId, int rolId)
        {
            var kullaniciRol = _unitOfWork.KullaniciRollerRepository.GetAll()
                .FirstOrDefault(kr => kr.KullaniciId == kullaniciId && kr.RolId == rolId);

            if (kullaniciRol == null) return false;

            var result = _unitOfWork.KullaniciRollerRepository.Delete(kullaniciRol);
            if (result)
            {
                _unitOfWork.SaveChanges();
            }

            return result;
        }

        public bool KullaniciRoluVarMi(int kullaniciId, string rolAdi)
        {
            var rol = _unitOfWork.RollerRepository.GetAll()
                .FirstOrDefault(r => r.RolAdi.ToLower() == rolAdi.ToLower());

            if (rol == null) return false;

            return _unitOfWork.KullaniciRollerRepository.GetAll()
                .Any(kr => kr.KullaniciId == kullaniciId && kr.RolId == rol.Id);
        }

        private RolDto MapToDto(Roller rol)
        {
            return new RolDto
            {
                Id = rol.Id,
                RolAdi = rol.RolAdi,
                Aciklama = rol.Aciklama,
                AktifMi = rol.AktifMi
            };
        }
    }
}
