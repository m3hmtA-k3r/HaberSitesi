using Application.DTOs;

namespace Business.Abstract
{
    public interface IRolService
    {
        List<RolDto> GetRoller();
        RolDto? GetRolById(int id);
        List<RolDto> GetKullaniciRolleri(int kullaniciId);
        bool AtaRol(int kullaniciId, int rolId);
        bool KaldirRol(int kullaniciId, int rolId);
        bool KullaniciRoluVarMi(int kullaniciId, string rolAdi);
    }
}
