using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Email;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public EmailService(IHttpClientFactory httpClientFactory,
             IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection("BackendUrlBase").Value;
            _client.BaseAddress = new Uri(baseUrl);
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
