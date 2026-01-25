using Application.DTOs;

namespace Business.Abstract
{
    public interface IKullaniciService
    {
        List<KullaniciDto> GetKullanicilar();
        KullaniciDto? GetKullaniciById(int id);
        KullaniciDto? GetKullaniciByEposta(string eposta);
        KullaniciDto CreateKullanici(KullaniciCreateDto model);
        KullaniciDto? UpdateKullanici(KullaniciUpdateDto model);
        bool DeleteKullanici(int id);
        bool ValidateCredentials(string eposta, string sifre);
        bool SifreDegistir(int kullaniciId, string eskiSifre, string yeniSifre);
    }
}
