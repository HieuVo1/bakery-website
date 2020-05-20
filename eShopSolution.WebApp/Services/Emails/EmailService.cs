using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Email;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        public EmailService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<ApiResult<string>> SendEmail(EmailMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/emails/SendMessage/", httpContent);
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(data);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(data);
        }
    }
}
