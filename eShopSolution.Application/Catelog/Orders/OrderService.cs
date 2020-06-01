using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ViewModel.Comment;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModel.Catalog.OrderDetails;

namespace eShopSolution.Application.Catelog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        public OrderService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<string>> Create(OrderCreateRequest request)
        {
            try
            {
                var order = new Order()
                {
                    Created_At = DateTime.Now,
                    OrderNotes = request.OrderNotes,
                    UserId = (request.UserId == null)?(Guid?)null:(new Guid(request.UserId)),
                    PromotionId = (request.PromotionId!=0)?request.PromotionId: (int?)null,
                    ShipName = request.ShipName,
                    ShipAddress = request.Street+" "+request.ShipAddress,
                    ShipEmail = request.ShipEmail,
                    ShipPhone = request.ShipPhone,
                    Total = request.Total,
                    OrderDetails = new List<OrderDetail>()
                };
                foreach (var item in request.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    order.OrderDetails.Add(orderDetail);
                }
                _context.Orders.Add(order);
                 await SaveChangeService.SaveChangeAsyncNotImage(_context);
                return new ApiResultSuccess<string>(order.Id.ToString());
            }
            catch(Exception ex)
            {
                return new ApiResultErrors<string>(ex.InnerException.Message);
            }
             
        }

        public async Task<ApiResult<bool>> Delete(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return new ApiResultErrors<bool>($"Can not find order with id: {orderId}");
            _context.Orders.Remove(order);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAll()
        {
            var query = from p in _context.Orders
                        join pr in _context.Promotions on p.PromotionId equals pr.Id
                        into ps
                        from pro in ps.DefaultIfEmpty()
                        select new { p,pro };
            var data = await query
             .Select(x => new OrderViewModel()
             {
                 Id = x.p.Id,
                 Created_At = x.p.Created_At,
                 OrderNotes = x.p.OrderNotes,
                 UserId = x.p.UserId.ToString(),
                 PromotionId = x.p.PromotionId ?? 0,
                 ShipName = x.p.ShipName,
                 ShipAddress = x.p.ShipAddress,
                 ShipEmail = x.p.ShipEmail,
                 ShipPhone = x.p.ShipPhone,
                 Total = x.p.Total,
                 Status = x.p.Status,
                 PromotionDiscount = x.pro.DiscountAmount
             }).ToListAsync();
            return new ApiResultSuccess<List<OrderViewModel>>(data);
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAllByUser(Guid userId)
        {
            var query = from p in _context.Orders
                        where p.UserId == userId
                        join pr in _context.Promotions on p.PromotionId equals pr.Id
                        into ps
                        from pro in ps.DefaultIfEmpty()
                        select new { p, pro };
                var data = await query
                 .Select(x => new OrderViewModel()
                 {
                     Id = x.p.Id,
                     Created_At = x.p.Created_At,
                     OrderNotes = x.p.OrderNotes,
                     UserId = x.p.UserId.ToString(),
                     PromotionId = x.p.PromotionId??0,
                     ShipName = x.p.ShipName,
                     ShipAddress = x.p.ShipAddress,
                     ShipEmail = x.p.ShipEmail,
                     ShipPhone = x.p.ShipPhone,
                     Total = x.p.Total,
                     Status = x.p.Status,
                     PromotionDiscount = x.pro.DiscountAmount
                 }).ToListAsync();
                return new ApiResultSuccess<List<OrderViewModel>>(data);
        }

        public async Task<ApiResult<OrderViewModel>> GetById(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return new ApiResultErrors<OrderViewModel>($"Can not find order with id: {orderId}");

            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                Created_At = order.Created_At,
                OrderNotes = order.OrderNotes,
                UserId = order.UserId.ToString(),
                PromotionId = order.PromotionId ?? 0,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipPhone = order.ShipPhone,
                Total = order.Total,
                Status = order.Status,
            };

            return new ApiResultSuccess<OrderViewModel>(orderViewModel);
        }

        public async Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetails(int orderId)
        {
            var query = from p in _context.OrderDetails
                        where p.OrderId == orderId
                        join pr in _context.ProductTranslations on p.ProductId  equals pr.ProductId
                        join img in _context.ProductImages on p.ProductId  equals img.ProductId
                        where img.IsDefault==true
                        select new { p,pr,img };
            var data = await query
             .Select(x => new OrderDetailViewModel()
             {
                 ProductName = x.pr.Name,
                 Quantity = x.p.Quantity,
                 Price = x.p.Price,
                 OrderId =x.p.OrderId,
                 ImagePath =x.img.ImagePath
             }).ToListAsync();
            return new ApiResultSuccess<List<OrderDetailViewModel>>(data);
        }

        public Task<ApiResult<bool>> Update(OrderUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> UpdateStatus(int status, int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return new ApiResultErrors<bool>($"Can not find order with id: {orderId}");
            switch (status)
            {
                case 0: order.Status = Data.Enums.OrderStatus.InProgress;break;
                case 1: order.Status = Data.Enums.OrderStatus.Shipping;break;
                case 2: order.Status = Data.Enums.OrderStatus.Delivered;break;
            }
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
    }
}
