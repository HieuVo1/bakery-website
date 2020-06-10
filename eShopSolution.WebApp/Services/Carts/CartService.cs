using eShopSolution.Application.Catelog.Carts;
using eShopSolution.ViewModel.Catalog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Carts
{
    public class CartService :BaseService, ICartService
    {
        public CartService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<string>> AddToCart(CartItemRequest request)
        {
            return await CreateAsync<ApiResult<string>, CartItemRequest>($"/api/carts/Items", request);
        }

        public async Task<ApiResult<string>> Create(CartCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>, CartCreateRequest>($"/api/carts", request);
        }

        public async Task<ApiResult<string>> Delete(int cartId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/carts/{cartId}");
        }

        public async Task<ApiResult<string>> DeleteAll(int cartId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/carts/DeleteAll?cartId={cartId}");
        }

        public async Task<ApiResult<string>> DeleteItem(CartItemRequest request)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/carts/Items?cartId={request.CartID}&productId={request.ProductID}&pricechange={request.PriceChange}");
        }

        public async Task<ApiResult<CartViewModel>> GetById(Guid userId)
        {
            return await GetAsync<ApiResult<CartViewModel>>($"/api/carts/{userId}");
        }

        public Task<ApiResult<string>> Update(CartUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> UpdateQuantity(CartItemRequest request)
        {
            return await UpdateAsync<ApiResult<string>, CartItemRequest>($"/api/carts/items", request);
   
        }
    }
}
