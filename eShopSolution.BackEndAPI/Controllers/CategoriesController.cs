using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Categories;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;
        public CategoriesController(ICategoryService CategoryService)
        {

            _CategoryService = CategoryService;
        }
        //https://locahost:port/api/categories/?PageIndex=1&pageSize=1
        [HttpGet("{LanguageId}")]
        public async Task<IActionResult> GetPagging(string LanguageId, [FromQuery] GetCategoryPaggingReqest request)
        {
            var result = await _CategoryService.GetAll( request, LanguageId);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://locahost:port/product/id
        [HttpGet("{categoryId}/{languageId}")]
        public async Task<IActionResult> GetById(int categoryId, string languageId)
        {
            var result = await _CategoryService.GetById(categoryId, languageId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _CategoryService.Create(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok( result);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateRequest request,int categoryId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _CategoryService.Update(request, categoryId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{categoryId}/{status}")]
        public async Task<IActionResult> UpdatePrice(int categoryId, CategoryStatus status)
        {
            var result = await _CategoryService.Updatestatus(categoryId, status);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var result = await _CategoryService.Delete(categoryId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

    }
}