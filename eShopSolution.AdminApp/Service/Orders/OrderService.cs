using eShopSolution.ViewModel.Catalog.OrderDetails;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Orders
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
            return await CreateAsync<ApiResult<string>, OrderCreateRequest>($"/api/orders", request);
        }

        public async Task<ApiResult<string>> Delete(int orderId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/orders/{orderId}");
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<OrderViewModel>>>($"/api/orders/getAll");
        }

        public async Task<ApiResult<OrderViewModel>> GetById(int orderId)
        {
            return await GetAsync<ApiResult<OrderViewModel>>($"/api/orders/{orderId}");
        }

        public async Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetail(int orderId)
        {
            return await GetAsync<ApiResult<List<OrderDetailViewModel>>>($"/api/orders/GetOrderDetails/{orderId}");
        }

        public Task<ApiResult<string>> Update(OrderUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> UpdateStatus(int status, int orderId)
        {
            return await GetAsync<ApiResult<string>>($"/api/orders/updateStatus/{orderId}/{status}");
        }
    }
}
