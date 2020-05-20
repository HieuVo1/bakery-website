using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Carts;
using eShopSolution.ViewModel.Catalog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUserId(Guid userId )
        {
            var result = await _cartService.GetById(userId);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CartCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.Create(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> Delete(int cartId)
        {
            var result = await _cartService.Delete(cartId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("Items")]
        public async Task<IActionResult> AddToCart(CartItemCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.AddToCart(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("Items")]
        public async Task<IActionResult> UpdateQuantity(CartItemUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.UpdateQuantity(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("Items")]
        public async Task<IActionResult> DeleteItem([FromQuery]int cartId,int productId)
        {
            var result = await _cartService.DeleteItem(cartId,productId);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
    }
}