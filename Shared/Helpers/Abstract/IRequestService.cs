namespace Shared.Helpers.Abstract
{
    public interface IRequestService
    {
        T Get<T>(string url);
        T Post<T>(string url, object? model);
        T Delete<T>(string url);
        T Put<T>(string url, object model);
    }
}
