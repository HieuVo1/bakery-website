using eShopSolution.ViewModel.Catalog.OrderDetails;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Orders
{
    public class OrderService :BaseService, IOrderService
    {
        public OrderService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }

        public async Task<ApiResult<string>> Create(OrderCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>,OrderCreateRequest>($"/api/orders", request);
        }

        public async Task<ApiResult<string>> Delete(int orderId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/orders/{orderId}");
            
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAllByUser(string userId)
        {
            return await GetAsync<ApiResult<List<OrderViewModel>>>($"/api/orders/GetAllByUser/{userId}");
        }

        public async Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetail(int orderId)
        {
            return await GetAsync<ApiResult<List<OrderDetailViewModel>>>($"/api/order/GetOrderDetails/{orderId}");
        }

        public Task<ApiResult<string>> Update(OrderUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
