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
    [Authorize(Roles = "admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;
        public ProductsController( IProductService manageProductService)
        {

            _ProductService = manageProductService;
        }

        //https://locahost:port/products/?PageIndex=1&pageSize=1&categoryId=1
        [HttpGet("{LanguageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagging(string LanguageId, [FromQuery] GetProductPaggingRequest request)
        {
            request.LanguageId = LanguageId;
            var products = await _ProductService.GetAllPagging(request);
            return Ok(products);
        }
        [HttpGet("Top/{LanguageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopSelling(string LanguageId, [FromQuery] GetProductPaggingRequest request)
        {
            request.LanguageId = LanguageId;
            var products = await _ProductService.GetTopSelling(request);
            return Ok(products);
        }
        //https://locahost:port/products/getbyUrl/languageId/?PageIndex=1&pageSize=1&categoryUrl=1
        [HttpGet("getByUrl/{LanguageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategoryUrl(string LanguageId, [FromQuery] GetProductPaggingRequest request)
        {
            var products = await _ProductService.GetAllByCategoryUrl(request, LanguageId);
            return Ok(products);
        }

        //https://locahost:port/products/productId/languageId
        [HttpGet("{productId}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _ProductService.GetById(productId, languageId);
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
            var result = await _ProductService.Create(request);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.Update(request);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice )
        {
            var result = await _ProductService.UpdatePrice(productId, newPrice);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _ProductService.Delete(productId);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        
    }
}