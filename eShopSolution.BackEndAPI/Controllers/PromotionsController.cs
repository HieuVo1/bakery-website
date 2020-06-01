using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Promotions;
using eShopSolution.ViewModel.Catalog.Promotions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetPagging()
        {
            var result = await _promotionService.GetAll();
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{promotionId}")]
        public async Task<IActionResult> GetById(int promotionId)
        {
            var result = await _promotionService.GetById(promotionId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("getcode/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _promotionService.GetByCode(code);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PromotionCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.Create(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{promotionId}")]
        public async Task<IActionResult> Update(PromotionUpdateRequest request, int promotionId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.Update(request, promotionId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{promotionId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int promotionId)
        {
            var result = await _promotionService.Delete(promotionId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
    }
}