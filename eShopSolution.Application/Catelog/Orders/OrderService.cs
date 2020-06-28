using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    TransactionId = request.TransactionId,
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
                var result = await SaveChangeService.SaveChangeAsyncNotImage(_context);
                if (result.IsSuccessed)
                {
                    return new ApiResultSuccess<string>(order.Id.ToString());
                }
                return new ApiResultErrors<string>(result.Message);
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
            var promotion = await _context.Promotions.FindAsync(order.PromotionId);
            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                Created_At = order.Created_At,
                OrderNotes = order.OrderNotes,
                UserId = order.UserId.ToString(),
                PromotionId = order.PromotionId ?? 0,
                PromotionDiscount = promotion==null ? 0:promotion.DiscountAmount,
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
                        where pr.LanguageId=="vn"
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

        public async Task<ApiResult<List<RevenueViewModel>>> GetRevenue()
        {
            var query = (from or in _context.Orders
                         where ((or.Created_At.Month < DateTime.Now.Month) && (or.Created_At.Year == DateTime.Now.Year))
                        
                         group or by new
                         {
                             or.Created_At.Month
                         } into result
                         select new
                         {
                             month = result.Key.Month,
                             total = (decimal?)result.Sum(p => p.Total)
                         })
                        .Union(from or in _context.Orders
                                where ((or.Created_At.Month >= DateTime.Now.Month) && (or.Created_At.Year == (DateTime.Now.Year-1)))
                               group or by new
                               {
                                   or.Created_At.Month
                               } into result
                               select new
                               {
                                   month = result.Key.Month,
                                   total = (decimal?)result.Sum(p => p.Total)
                               });
            var data = await query
            .Select(x => new RevenueViewModel
            { 
                Month = (x.month<DateTime.Now.Month)?(x.month+ DateTime.Now.Month): (x.month - DateTime.Now.Month),
                Total =x.total
                 
            }).ToListAsync();
            return new ApiResultSuccess<List<RevenueViewModel>>(data);
        }

        public async Task<ApiResult<List<RevenueByCategory>>> GetRevenueByCategory(int take)
        {
            var query = from or in _context.Orders
                        join od in _context.OrderDetails on or.Id equals od.OrderId
                        join pro in _context.Products on od.ProductId equals pro.Id
                        join ca in _context.CategoryTranslations on pro.CategoryId equals ca.CategoryId
                        where ca.LanguageId == "vn"
                        select new { or, ca }
                        into re
                        group re by re.ca.Name
                        into result
                        orderby
                         (decimal?)result.Sum(p => p.or.Total) descending
                        select new
                        {
                            catelogyName = result.Key,
                            total = (decimal?)result.Sum(p => p.or.Total)
                        };
            var data = await query.Take(take)
               .Select(x => new RevenueByCategory()
               {
                 CategoryName = x.catelogyName,
                 Total=x.total
               }).ToListAsync();

            return new ApiResultSuccess<List<RevenueByCategory>>(data);
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
