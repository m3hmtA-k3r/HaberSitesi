using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IAuthApiRequest
    {
        LoginResponse Login(string eposta, string sifre);
        ProfilDto? GetProfil();
        ProfilDto? UpdateProfil(ProfilGuncelleDto model);
        bool SifreDegistir(SifreDegistirDto model);
    }
}
