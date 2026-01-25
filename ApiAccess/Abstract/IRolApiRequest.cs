using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IRolApiRequest
    {
        List<RolDto> GetAllRol();
        RolDto? GetRolById(int id);
    }
}
