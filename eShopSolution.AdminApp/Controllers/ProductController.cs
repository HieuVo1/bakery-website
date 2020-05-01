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
            var result = await _languageService.GetAll();
            var categories = await _categoryService.GetAll("vn");
            ViewData["languages"] = result.ResultObject;
            var indexVN = result.ResultObject.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != -1)
            {
                SwapGeneric<LanguageViewModel>.Swap(result.ResultObject, indexVN, 0);
            }
            ViewData["categories"] = categories.ResultObject;
            ViewData["products"] = products.ResultObject.Items;
            if (TempData["result"] != null)
            {
                ViewBag.result = TempData["result"];
                ViewBag.IsSuccess = TempData["IsSuccess"];
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> NewProduct(string categoryUrl)
        {
            var result = await _languageService.GetAll();
            var categories = await _categoryService.GetAll("vn");
            ViewData["languages"] = result.ResultObject;
            
            var indexVN = result.ResultObject.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != 0)
            {
                SwapGeneric<LanguageViewModel>.Swap(result.ResultObject, indexVN, 0);
            }
            ViewData["categories"] = categories.ResultObject;
            var index = categories.ResultObject.FindIndex(x => x.CategoryUrl == categoryUrl);
            //Swap
            if (index != 0)
            {
                SwapGeneric<CategoryViewModel>.Swap(categories.ResultObject, index, 0);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var result = await _productServive.Create(request);
                if (result.IsSuccessed == true)
                {
                    TempData["result"] = "Create Success";
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["result"] = result.Message;
                    TempData["IsSuccess"] = false;
                }
                var category = await _categoryService.GetById(request.CategoryId, "vn");
                return Redirect($"/product/{category.ResultObject.CategoryUrl}");

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
            var result = await _productServive.Delete(id);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = "Delete Success";
                TempData["IsSuccess"] = true;
            }
            else
            {
                TempData["result"] = result.Message;
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("Index", "product", new { categoryUrl = categoryUrl });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int productId, string categoryUrl,string languageId)
        {
            var product = await _productServive.GetById(productId,languageId);
            var result = await _languageService.GetAll();
            var categories = await _categoryService.GetAll("vn");
            ViewData["languages"] = result.ResultObject;

            var indexVN = result.ResultObject.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != 0)
            {
                SwapGeneric<LanguageViewModel>.Swap(result.ResultObject, indexVN, 0);
            }
            ViewData["categories"] = categories.ResultObject;
            var index = categories.ResultObject.FindIndex(x => x.CategoryUrl == categoryUrl);
            //Swap
            if (index != 0)
            {
                SwapGeneric<CategoryViewModel>.Swap(categories.ResultObject, index, 0);
            }
            ViewData["product"] = product.ResultObject;
            return View("NewProduct");
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request, [FromRoute]int Id)
        {

            if (ModelState.IsValid)
            {
                request.Id = Id;
                var result = await _productServive.Update(request);
                if (result.IsSuccessed == true)
                {
                    TempData["result"] = "Update Success";
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["result"] = result.Message;
                    TempData["IsSuccess"] = false;
                }
                var category = await _categoryService.GetById(request.CategoryId, "vn");
                return Redirect($"/product/{category.ResultObject.CategoryUrl}");
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }

    }
}