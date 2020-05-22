using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Reviews;
using eShopSolution.ViewModel.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet("getAll/{productId}")]
        public async Task<IActionResult> GetPagging(int productId)
        {
            var result = await _reviewService.GetAll(productId);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetById(int reviewId)
        {
            var result = await _reviewService.GetById(reviewId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _reviewService.Create(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{reviewId}")]
        public async Task<IActionResult> Update(ReviewUpdateRequest request, int reviewId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _reviewService.Update(request, reviewId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var result = await _reviewService.Delete(reviewId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
    }
}