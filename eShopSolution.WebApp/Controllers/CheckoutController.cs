using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Carts;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Catalog.OrderDetails;
using eShopSolution.ViewModel.Catalog.Orders;
using eShopSolution.WebApp.Helpers;
using eShopSolution.WebApp.Services.Location;
using eShopSolution.WebApp.Services.Orders;
using eShopSolution.WebApp.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly ILocationService _locationService;
        private readonly IOrderService _orderService;
        private readonly IUserAPIClient _userAPIClient;
        private readonly ICartService _cartService;

        public CheckoutController(ILocationService locationService,
            IConfiguration configuration,
            IUserAPIClient userAPIClient,
            ICartService cartService,
            IOrderService orderService):base(configuration)
        {
            _locationService = locationService;
            _orderService = orderService;
            _userAPIClient = userAPIClient;
            _cartService = cartService;
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(int PromotionId,int PromotionPrice)
        {
            if (section != null)
            {
                var userinfo = await _userAPIClient.GetUserById(new Guid(ViewBag.UserId));
                if (userinfo.IsSuccessed)
                {
                    ViewBag.UserInfo = userinfo.ResultObject;
                }
            }
            
            ViewBag.PromotionId = PromotionId;
            ViewBag.PromotionPrice = PromotionPrice;
            return View();
        }
        public async Task<IActionResult> GetLocationAsync(string keyword)
        {
            var result = await _locationService.GetAll(keyword);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Order(OrderCreateRequest request)
        {
            request.OrderDetails = new List<OrderDetailCreateRequest>();
            foreach (var item in ViewBag.cart)
            {
                var detail = new OrderDetailCreateRequest
                {
                    ProductId = item.Product.Id,
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                };
                request.OrderDetails.Add(detail);
            }
            if (ModelState.IsValid)
            {
                var result = await _orderService.Create(request);
                if (result.IsSuccessed)
                {
                    var data = new
                    {
                        Id = result.ResultObject,
                        Name = request.ShipName,
                        Phone = request.ShipPhone,
                        Address = request.ShipAddress,
                        Total = request.Total,
                        Status = OrderStatus.InProgress.ToString(),
                        Create_At = DateTime.Now.ToShortDateString(),
                    };
                    await ChannelHelper.Trigger(data, "feed", "new_feed", _configuration);
                    var CartSessionKey = _configuration.GetSection("CartSessionKey").Value;
                    List<CartItemViewModel> cart = new List<CartItemViewModel>();
                    if (ViewBag.CartId != null)
                    {
                        await _cartService.DeleteAll(Convert.ToInt32(ViewBag.CartId));
                    }
                    HttpContext.Session.Remove(CartSessionKey);
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError(string.Empty, result.Message);
                return View("index");
            }
            else
            {
                return View("index");
            }
        }
    }
}