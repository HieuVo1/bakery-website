using System;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Orders;
using eShopSolution.ViewModel.Catalog.Orders;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("GetAllByUser/{userId}")]
        public async Task<IActionResult> GetAllByUser(Guid userId)
        {
            var result = await _orderService.GetAllByUser(userId);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var result = await _orderService.GetById(orderId);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAll();
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://locahost:port/comment/id
        [HttpGet("GetOrderDetails/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var result = await _orderService.GetOrderDetails(orderId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _orderService.Create(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> Update(OrderUpdateRequest request, int orderId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _orderService.Update(request);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> Delete(int orderId)
        {
            var result = await _orderService.Delete(orderId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("updateStatus/{orderId}/{status}")]
        public async Task<IActionResult> UpdateStatus(int orderId,int status)
        {
            var result = await _orderService.UpdateStatus(status,orderId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
    }
}