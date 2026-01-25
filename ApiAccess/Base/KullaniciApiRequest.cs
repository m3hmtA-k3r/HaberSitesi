using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class KullaniciApiRequest : IKullaniciApiRequest
    {
        private readonly IRequestService _requestService;

        public KullaniciApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public List<KullaniciDto> GetAllKullanici()
        {
            return _requestService.Get<List<KullaniciDto>>("Kullanici");
        }

        public KullaniciDto? GetKullaniciById(int id)
        {
            return _requestService.Get<KullaniciDto>($"Kullanici/{id}");
        }

        public KullaniciDto CreateKullanici(KullaniciCreateDto model)
        {
            return _requestService.Post<KullaniciDto>("Kullanici", model);
        }

        public KullaniciDto? UpdateKullanici(KullaniciUpdateDto model)
        {
            return _requestService.Put<KullaniciDto>($"Kullanici/{model.Id}", model);
        }

        public bool DeleteKullanici(int id)
        {
            return _requestService.Delete<bool>($"Kullanici/{id}");
        }

        public bool AtaRol(int kullaniciId, int rolId)
        {
            var result = _requestService.Post<dynamic>($"Kullanici/{kullaniciId}/rol/{rolId}", null);
            return result != null;
        }

        public bool KaldirRol(int kullaniciId, int rolId)
        {
            return _requestService.Delete<bool>($"Kullanici/{kullaniciId}/rol/{rolId}");
        }

        public List<RolDto> GetKullaniciRolleri(int kullaniciId)
        {
            return _requestService.Get<List<RolDto>>($"Kullanici/{kullaniciId}/roller");
        }
    }
}
