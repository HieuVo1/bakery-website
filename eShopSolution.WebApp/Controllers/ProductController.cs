using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.WebApp.Services.Categorys;
using eShopSolution.WebApp.Services.Languages;
using eShopSolution.WebApp.Services.products;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly int _pageSize = 6;
        public ProductController(IProductService productService, ICategoryService categoryService,ILanguageService languageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _languageService = languageService;
        }
        public async Task<IActionResult> IndexAsync([FromQuery] int minPrice, int maxPrice,int pageIndex=1,string Name=null)
        
        {
            var products = await _productService.GetAll("vn", Name, pageIndex, 1,minPrice,maxPrice);
            var categories = await _categoryService.GetAll("vn");
            ViewData["products"] = products.ResultObject.Items;
            ViewData["minPrice"] = minPrice;
            ViewData["maxPrice"] = maxPrice;
            ViewData["categories"] = categories.ResultObject;
            return View(products.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> GetByURl(string categoryUrl, [FromQuery] int page = 1)
        {
            var products = await _productService.GetByCategoryUrl( "vn", categoryUrl, page,_pageSize);
            var categories = await _categoryService.GetAll("vn");
            ViewData["products"] = products.ResultObject.Items;
            ViewData["categories"] = categories.ResultObject;
            ViewData["total"] = products.ResultObject.TotalRecords;
            ViewData["NumPage"] =(products.ResultObject.TotalRecords % _pageSize==0)?products.ResultObject.TotalRecords / _pageSize: products.ResultObject.TotalRecords / _pageSize+1;
            return View("Index");
        }
    }
}