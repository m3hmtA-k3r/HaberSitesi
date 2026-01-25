using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
    public class RolApiRequest : IRolApiRequest
    {
        private readonly IRequestService _requestService;

        public RolApiRequest(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public List<RolDto> GetAllRol()
        {
            return _requestService.Get<List<RolDto>>("Rol");
        }

        public RolDto? GetRolById(int id)
        {
            return _requestService.Get<RolDto>($"Rol/{id}");
        }
    }
}
