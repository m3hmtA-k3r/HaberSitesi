using Application.DTOs;

namespace Business.Abstract
{
    public interface IAuthService
    {
        (KullaniciDto? Kullanici, string? Token, string? Hata) Login(string eposta, string sifre);
        ProfilDto? GetProfil(int kullaniciId);
        ProfilDto? UpdateProfil(int kullaniciId, ProfilGuncelleDto model);
        bool SifreDegistir(int kullaniciId, SifreDegistirDto model);
    }
}
