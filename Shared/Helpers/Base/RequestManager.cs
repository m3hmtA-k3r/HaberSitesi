using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Shared.Helpers.Abstract;

namespace Shared.Helpers.Base
{
    public class RequestManager : IRequestService
    {
        private readonly string _baseApiAddress;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _baseApiAddress = configuration["Ayarlar:ApiUrl"] ?? "http://localhost:5100/api/";
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader(RestRequest request)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }
        }

        public T Get<T>(string url)
        {
            var client = new RestClient(_baseApiAddress);
            var request = new RestRequest(url);
            AddAuthorizationHeader(request);
            var response = client.ExecuteGet(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                return default!;

            return JsonConvert.DeserializeObject<T>(response.Content) ?? default!;
        }

        public T Post<T>(string url, object? model)
        {
            var client = new RestClient(_baseApiAddress);
            var request = new RestRequest(url);
            AddAuthorizationHeader(request);

            // Only add body if model is not null
            if (model != null)
            {
                request.AddJsonBody(model);
            }

            var response = client.ExecutePost(request);

            if (string.IsNullOrEmpty(response.Content))
                return default!;

            // Parse response content even on error status codes
            // to capture error messages (e.g. LoginResponse with Success=false)
            if (!response.IsSuccessful)
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(response.Content) ?? default!;
                }
                catch
                {
                    return default!;
                }
            }

            return JsonConvert.DeserializeObject<T>(response.Content) ?? default!;
        }

        public T Delete<T>(string url)
        {
            var client = new RestClient(_baseApiAddress);
            var request = new RestRequest(url);
            AddAuthorizationHeader(request);
            var response = client.ExecuteDelete(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                return default!;

            return JsonConvert.DeserializeObject<T>(response.Content) ?? default!;
        }

        public T Put<T>(string url, object model)
        {
            var client = new RestClient(_baseApiAddress);
            var request = new RestRequest(url);
            AddAuthorizationHeader(request);
            request.AddJsonBody(model);
            var response = client.ExecutePut(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                return default!;

            return JsonConvert.DeserializeObject<T>(response.Content) ?? default!;
        }
    }
}
