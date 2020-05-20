using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.WebApp.Helpers;
using eShopSolution.WebApp.Services.Categorys;
using eShopSolution.WebApp.Services.ImageProducts;
using eShopSolution.WebApp.Services.Languages;
using eShopSolution.WebApp.Services.products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly IImageProductService _imageProductService;
        private readonly int _pageSize = 6;
        public ProductController(IProductService productService, 
            ICategoryService categoryService,
            ILanguageService languageService,
            IImageProductService imageProductService,
            IConfiguration configuration):base(configuration)
        {
            _productService = productService;
            _categoryService = categoryService;
            _languageService = languageService;
            _imageProductService = imageProductService;
        }
        public async Task<IActionResult> IndexAsync([FromQuery] int minPrice, int maxPrice,int pageIndex=1,string Name=null)
        
        {
            var products = await _productService.GetAll("vn", Name, pageIndex, _pageSize, minPrice,maxPrice);
            var categories = await _categoryService.GetAll("vn");
            ViewData["products"] = products.ResultObject.Items;
            ViewData["minPrice"] = minPrice;
            ViewData["maxPrice"] = maxPrice;
            ViewData["categories"] = categories.ResultObject;
            if (section != null)
            {
                var cartId = CookieHelpers.GetObjectFromJson<string>(Request.Cookies, "CartId");
                ViewBag.IsLogged = true;
                ViewBag.CartId = cartId;
            }
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
            return View("Index", products.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> DetailAsync(int productId)
        {
            var result = await _productService.GetById(productId, languageDefauleId);
            var images = await _imageProductService.GetListImage(productId);
            ViewBag.ListImage = images.ResultObject;
            return View(result.ResultObject);
        }
    }
}