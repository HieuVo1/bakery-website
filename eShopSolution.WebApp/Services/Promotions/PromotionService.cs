using eShopSolution.ViewModel.Catalog.Promotions;
using eShopSolution.ViewModel.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Promotions
{
    public class PromotionService : IPromotionService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private readonly IConfiguration _configuration;
        private HttpClient _client;
        public PromotionService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection("BackendUrlBase").Value;
            _client.BaseAddress = new Uri(baseUrl);
        }
       
        public async Task<ApiResult<string>> Delete(int promotionId)
        {
            var response = await _client.DeleteAsync($"/api/promotions/{promotionId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        
        public async Task<ApiResult<PromotionViewModel>> GetByCode(string code)
        {
            var response = await _client.GetAsync($"/api/promotions/getcode/{code}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<PromotionViewModel>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<PromotionViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }
    }
}
