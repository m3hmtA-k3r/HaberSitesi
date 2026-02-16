using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class AuthApiRequest : IAuthApiRequest
    {
        private readonly IRequestService _requestService;

        public AuthApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public LoginResponse Login(string eposta, string sifre)
        {
            var request = new { Eposta = eposta, Sifre = sifre };
            return _requestService.Post<LoginResponse>("Auth/login", request);
        }

        public ProfilDto? GetProfil()
        {
            return _requestService.Get<ProfilDto>("Auth/profil");
        }

        public ProfilDto? UpdateProfil(ProfilGuncelleDto model)
        {
            return _requestService.Put<ProfilDto>("Auth/profil", model);
        }

        public bool SifreDegistir(SifreDegistirDto model)
        {
            var result = _requestService.Post<dynamic>("Auth/sifre-degistir", model);
            return result?.success ?? false;
        }

        public LoginResponse Register(string ad, string soyad, string eposta, string sifre)
        {
            var request = new { Ad = ad, Soyad = soyad, Eposta = eposta, Sifre = sifre };
            return _requestService.Post<LoginResponse>("Auth/register", request);
        }
    }
}
