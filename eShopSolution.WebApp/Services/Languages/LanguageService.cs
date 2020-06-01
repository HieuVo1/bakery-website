using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private readonly IConfiguration _configuration;
        public LanguageService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection("BackendUrlBase").Value;
            _client.BaseAddress = new Uri(baseUrl);
        }


        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            var response = await _client.GetAsync($"/api/languages");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<List<LanguageViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<List<LanguageViewModel>>>(data);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<LanguageViewModel>> GetById(string languageId)
        {
            var response = await _client.GetAsync($"/api/languages/{languageId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<LanguageViewModel>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<LanguageViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

    }
}
