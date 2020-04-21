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
        private readonly IPublicCategoryService _publicCategoryService;
        private readonly IManageCategoryService _manageCategoryService;
        public CategoriesController(IPublicCategoryService publicCategoryService, IManageCategoryService manageCategoryService)
        {
            _publicCategoryService = publicCategoryService;
            _manageCategoryService = manageCategoryService;
        }
        //https://locahost:port/api/categories/?PageIndex=1&pageSize=1
        [HttpGet("{LanguageId}")]
        public async Task<IActionResult> GetPagging(string LanguageId, [FromQuery] GetCategoryPaggingReqest request)
        {
            var categorys = await _publicCategoryService.GetAll( request, LanguageId);
            return Ok(categorys);
        }
        //https://locahost:port/product/id
        [HttpGet("{categoryId}/{languageId}")]
        public async Task<IActionResult> GetById(int categoryId, string languageId)
        {
            var category = await _manageCategoryService.GetById(categoryId, languageId);
            if (category == null) return NotFound("Can not find");
            return Ok(category);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageCategoryService.Create(request);
            if (result == null) return BadRequest();
            return Ok( result);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateRequest request,int categoryId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageCategoryService.Update(request, categoryId);
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPatch("{categoryId}/{status}")]
        public async Task<IActionResult> UpdatePrice(int categoryId, CategoryStatus status)
        {
            var result = await _manageCategoryService.Updatestatus(categoryId, status);
            if (result == false) return BadRequest();
            return Ok();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var result = await _manageCategoryService.Delete(categoryId);
            if (result == null) return BadRequest();
            return Ok(result);
        }

    }
}