using eShopSolution.ViewModel.Catalog.OrderDetails;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Orders
{
    public interface IOrderService
    {
        Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetails(int orderId);
        Task<ApiResult<OrderViewModel>> GetById(int orderId);
        Task<ApiResult<List<OrderViewModel>>> GetAllByUser(Guid userId);
        Task<ApiResult<List<OrderViewModel>>> GetAll();
        Task<ApiResult<string>> Create(OrderCreateRequest request);
        Task<ApiResult<bool>> Update(OrderUpdateRequest request);
        Task<ApiResult<bool>> UpdateStatus(int status, int orderID);
        Task<ApiResult<bool>> Delete(int orderId);
    }
}
