using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.ImageProducts;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.AdminApp.Service.Products;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Language;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productServive;
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ILanguageService languageService,ICategoryService categoryService)
        {
            _languageService = languageService;
            _productServive = productService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index( string categoryUrl)
        {
            var products = await _productServive.GetAllByCategoryUrl(categoryUrl,"vn");
            var languages = await _languageService.GetAll();
            var categories = await _categoryService.GetAll("vn");
            ViewData["languages"] = languages;
            var indexVN = languages.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != 0)
            {
                SwapGeneric<LanguageViewModel>.Swap(languages, indexVN, 0);
            }
            ViewData["categories"] = categories;
            ViewData["products"] = products.Items;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> NewProduct(string categoryUrl)
        {
            var languages = await _languageService.GetAll();
            var categories = await _categoryService.GetAll("vn");
            ViewData["languages"] = languages;
            
            var indexVN = languages.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != 0)
            {
                SwapGeneric<LanguageViewModel>.Swap(languages, indexVN, 0);
            }
            ViewData["categories"] = categories;
            var index = categories.FindIndex(x => x.CategoryUrl == categoryUrl);
            //Swap
            if (index != 0)
            {
                SwapGeneric<CategoryViewModel>.Swap(categories, index, 0);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var isCreated = await _productServive.Create(request);
                if (isCreated == true)
                {
                    var category = await _categoryService.GetById(request.CategoryId,"vn");
                    return Redirect($"/product/{category.CategoryUrl}");
                }
                return BadRequest();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        [Route("{categoryUrl}/{id}")]
        public async Task<IActionResult> Delete( int id, string categoryUrl)
        {
            var IsDeleted = await _productServive.Delete(id);
            if (IsDeleted == true) return RedirectToAction("Index", "product", new { categoryUrl = categoryUrl });
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int productId, string categoryUrl,string languageId)
        {
            var product = await _productServive.GetById(productId,languageId);
            var languages = await _languageService.GetAll();
            var categories = await _categoryService.GetAll("vn");
            ViewData["languages"] = languages;

            var indexVN = languages.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != 0)
            {
                SwapGeneric<LanguageViewModel>.Swap(languages, indexVN, 0);
            }
            ViewData["categories"] = categories;
            var index = categories.FindIndex(x => x.CategoryUrl == categoryUrl);
            //Swap
            if (index != 0)
            {
                SwapGeneric<CategoryViewModel>.Swap(categories, index, 0);
            }
            ViewData["product"] = product;
            return View("NewProduct");
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request, [FromRoute]int Id)
        {

            if (ModelState.IsValid)
            {
                request.Id = Id;
                var isCreated = await _productServive.Update(request);
                if (isCreated == true)
                {
                    var category = await _categoryService.GetById(request.CategoryId, "vn");
                    return Redirect($"/product/{category.CategoryUrl}");
                }
                return BadRequest();
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }

    }
}