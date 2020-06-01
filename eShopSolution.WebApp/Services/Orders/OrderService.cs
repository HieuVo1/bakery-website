using eShopSolution.ViewModel.Catalog.OrderDetails;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private HttpClient _client;
        public OrderService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection("BackendUrlBase").Value;
            _client.BaseAddress = new Uri(baseUrl);
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<ApiResult<string>> Create(OrderCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/orders", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> Delete(int orderId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/orders/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAllByUser(string userId)
        {
            var response = await _client.GetAsync($"/api/orders/GetAllByUser/{userId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<List<OrderViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<List<OrderViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetail(int orderId)
        {
            var response = await _client.GetAsync($"/api/order/GetOrderDetails/{orderId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<List<OrderDetailViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<List<OrderDetailViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public Task<ApiResult<string>> Update(OrderUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
