using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Products;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        //https://locahost:port/products/?PageIndex=1&pageSize=1&categoryId=1
        [HttpGet("{LanguageId}")]
        [Authorize]
        public async Task<IActionResult> GetPagging(int LanguageId, [FromQuery] GetProductPublicPaggingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request, LanguageId);
            return Ok(products);
        }

        //https://locahost:port/product/id
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId,int languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
            if (product == null) return NotFound("Can not find");
            return Ok(product);
        }

      
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.Create(request);
            if (result == 0) return BadRequest();
            return Created(nameof(GetById), result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.Update(request);
            if (result == 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice )
        {
            var result = await _manageProductService.UpdatePrice(productId, newPrice);
            if (result==false) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _manageProductService.Delete(productId);
            if (result == 0) return BadRequest();
            return Ok();
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImages(int productId,[FromForm] ProductImageCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.AddImage(productId, request);
            if (result == 0) return BadRequest();
            return Created(nameof(GetById), result);
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId,int imageId)
        {
            var product = await _manageProductService.GetImageById(imageId);
            if (product == null) return NotFound("Can not find");
            return Ok(product);
        }
        [HttpGet("{productId}/images")]
        public async Task<IActionResult> GetAllImage(int productId)
        {
            var product = await _manageProductService.GetListImage(productId);
            return Ok(product);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.UpdateImage(imageId, request);
            if (result == 0) return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = await _manageProductService.RemoveImage(imageId);
            if (result == 0) return BadRequest();
            return Ok();
        }
    }
}