using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.AdminApp.Service.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(ICategoryService categoryService,
            ILanguageService languageService,
            IConfiguration configuration,
            IOrderService orderService) : base(languageService, categoryService, configuration)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> IndexAsync()
        
        {
            var result = await _orderService.GetAll();
            if (result.IsSuccessed)
            {
                ViewData["orders"] = result.ResultObject;
            }
            return View();
        }
        public async Task<IActionResult> Detail(int orderId)
        {
            var result = await _orderService.GetOrderDetail(orderId);
            var order = await _orderService.GetById(orderId);
            if (result.IsSuccessed)
            {
                ViewData["orderDetails"] = result.ResultObject;
                ViewData["order"] = order.ResultObject;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int orderId)
        {
            var result = await _orderService.Delete(orderId);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = "Delete Success";
                TempData["IsSuccess"] = true;
            }
            else
            {
                TempData["result"] = result.Message;
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("Index", "order");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, int status)
        {
            var result = await _orderService.UpdateStatus(status,orderId);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = "update Success";
                TempData["IsSuccess"] = true;
            }
            else
            {
                TempData["result"] = result.Message;
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("Detail", "order", new { orderId= orderId });
        }
    }
}