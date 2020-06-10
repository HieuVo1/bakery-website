using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service
{
    public class BaseService
    {
        protected readonly IHttpClientFactory _httpClientFactor;
        private readonly IConfiguration _configuration;
        protected HttpClient _client;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection(SystemConstants.BackendUrlBase).Value;
            _client.BaseAddress = new Uri(baseUrl);
        }
        protected async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.GetAsync(url);
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<TResponse>(data);
                    }
                    return JsonConvert.DeserializeObject<TResponse>(data);

                }
                else
                {
                    return JsonConvert.DeserializeObject<TResponse>(null);
                }
            }

        }
        protected async Task<TResponse> DeleteAsync<TResponse>(string url)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }
        protected async Task<TResponse> CreateAsync<TResponse, TRequest>(string url, TRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(result);
            }
            return JsonConvert.DeserializeObject<TResponse>(result);
        }
        
        protected async Task<TResponse> UpdateAsync<TResponse, TRequest>(string url, TRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync(url, httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(result);
            }
            return JsonConvert.DeserializeObject<TResponse>(result);
        }
        protected async Task<TResponse> UpdateWithImageAsync<TResponse>(string url, MultipartFormDataContent form)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.PatchAsync(url, form);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(result);
            }
            return JsonConvert.DeserializeObject<TResponse>(result);
        }
        protected async Task<TResponse> CreateWithImageAsync<TResponse>(string url, MultipartFormDataContent form)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.PostAsync(url, form);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(result);
            }
            return JsonConvert.DeserializeObject<TResponse>(result);
        }
    }
}
