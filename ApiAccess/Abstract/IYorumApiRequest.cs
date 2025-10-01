using Shared.Dtos;

namespace ApiAccess.Abstract
{
    public interface IYorumApiRequest
    {
        List<YorumlarDto> GetAllYorum();
        YorumlarDto GetYorumById(int yorumId);
        YorumlarDto InsertYorum(YorumlarDto model);
        YorumlarDto UpdateYorum(YorumlarDto model);
        bool DeleteYorum(int yorumId);
    }
}
