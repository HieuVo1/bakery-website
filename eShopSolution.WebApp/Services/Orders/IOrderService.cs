using eShopSolution.ViewModel.Catalog.OrderDetails;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Orders
{
    public interface IOrderService
    {
        Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetail(int orderId);
        Task<ApiResult<List<OrderViewModel>>> GetAllByUser(string userId);
        Task<ApiResult<string>> Create(OrderCreateRequest request);
        Task<ApiResult<string>> Update(OrderUpdateRequest request);
        Task<ApiResult<string>> Delete(int orderId);
    }
}
