using eShopSolution.Application.Catelog.Carts;
using eShopSolution.ViewModel.Catalog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Carts
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;
        public CartService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<ApiResult<string>> AddToCart(CartItemCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/carts/Items", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> Create(CartCreateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/carts", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> Delete(int cartId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/carts/{cartId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<string>> DeleteItem(int cartId, int productId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/carts/Items?cartId={cartId}&productId={productId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<CartViewModel>> GetById(Guid userId)
        {
            var response = await _client.GetAsync($"/api/carts/{userId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<CartViewModel>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<CartViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public Task<ApiResult<string>> Update(CartUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> UpdateQuantity(CartItemUpdateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/carts/items", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }
    }
}
