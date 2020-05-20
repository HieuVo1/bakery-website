using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Contact;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        public ContactService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<ApiResult<string>> Create(ContactViewModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/contacts", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }
    }
}
