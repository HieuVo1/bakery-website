using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.ProductImages;
using eShopSolution.ViewModel.Catalog.ProductImages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImages(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _productImageService.AddImage(productId, request);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{productId}/images/{imageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var result = await _productImageService.GetImageById(imageId);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("{productId}/images")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllImage(int productId)
        {
            var result = await _productImageService.GetListImage(productId);
            return Ok(result);
        }

        [HttpPatch("images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _productImageService.UpdateImage(imageId, request);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = await _productImageService.RemoveImage(imageId);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }
    }
}