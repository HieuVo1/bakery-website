using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Review;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private readonly IConfiguration _configuration;
        public ReviewService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection("BackendUrlBase").Value;
            _client.BaseAddress = new Uri(baseUrl);
        }
        public async Task<ApiResult<string>> Create(ReviewCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/reviews", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> Delete(int reviewId)
        {
            var response = await _client.DeleteAsync($"/api/reviews/{reviewId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<List<ReviewViewModel>>> GetAll(int productId, int pageIndex = 0, int pageSize = 0)
        {
            var response = await _client.GetAsync($"/api/reviews/getAll/{productId}?pageIndex={pageIndex}&pageSize={pageSize}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<List<ReviewViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<List<ReviewViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public Task<ApiResult<ReviewViewModel>> GetById(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> Update(ReviewUpdateRequest request, int reviewId)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/api/reviews/{reviewId}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }
    }
}
