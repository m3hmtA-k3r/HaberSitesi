using Newtonsoft.Json;
using RestSharp;
using Shared.Helpers.Abstract;

namespace Shared.Helpers.Base
{

    public class RequestManager : IRequestService
    {
        readonly string baseApiAddress = "https://localhost:7063/api/";
        public T Get<T>(string url)
        {
            var client = new RestClient(baseApiAddress);
            var request = new RestRequest(url);
            var response = client.ExecuteGet(request);

            var result = JsonConvert.DeserializeObject<T>(response.Content);
            return result;
        }

        public T Post<T>(string url, object model)
        {
            var client = new RestClient(baseApiAddress);
            var request = new RestRequest(url);
            request.AddBody(model);
            var response = client.ExecutePost(request);

            var result = JsonConvert.DeserializeObject<T>(response.Content);
            return result;
        }
    }
}
