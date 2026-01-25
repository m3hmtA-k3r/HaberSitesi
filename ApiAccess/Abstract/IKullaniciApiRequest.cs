using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IKullaniciApiRequest
    {
        List<KullaniciDto> GetAllKullanici();
        KullaniciDto? GetKullaniciById(int id);
        KullaniciDto CreateKullanici(KullaniciCreateDto model);
        KullaniciDto? UpdateKullanici(KullaniciUpdateDto model);
        bool DeleteKullanici(int id);
        bool AtaRol(int kullaniciId, int rolId);
        bool KaldirRol(int kullaniciId, int rolId);
        List<RolDto> GetKullaniciRolleri(int kullaniciId);
    }
}
