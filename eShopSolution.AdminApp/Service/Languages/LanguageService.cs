using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Language;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        public LanguageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<bool> Create(LanguageCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/languages", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(string languageId)
        {
            var response = await _client.DeleteAsync($"/api/languages/{languageId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            var response = await _client.GetAsync($"/api/languages");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    return JsonConvert.DeserializeObject<List<LanguageViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<LanguageViewModel> GetById(string languageId)
        {
            var response = await _client.GetAsync($"/api/languages/{languageId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    return JsonConvert.DeserializeObject<LanguageViewModel>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> Update(LanguageUpdateRequest request, string languageId)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/languages/{languageId}", httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}
