using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.ImageProducts;
using eShopSolution.AdminApp.Service.Products;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{

    public class ImageProductController : BaseController
    {
        private readonly IProductService _productServive;
        private readonly IImageProductService _imageProductServive;
        private readonly ICategoryService _categoryService;
        public ImageProductController(IProductService productService,ICategoryService categoryService, IImageProductService imageProductServive)
        {
            _productServive = productService;
            _imageProductServive = imageProductServive;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> IndexAsync(int productId)
        {
            var categories = await _categoryService.GetAll("vn");
            var paggingRequest = new GetProductPaggingRequest()
            {
                LanguageId = "vn",
            };
            var products = await _productServive.getAllPagging(paggingRequest);
            var images = await _imageProductServive.GetListImage(productId);
            var productSelected = products.ResultObject.Items.FindIndex(x => x.Id == productId);
            if (productSelected != -1)
            {
                SwapGeneric<ProductViewModel>.Swap(products.ResultObject.Items, productSelected, 0);
            }
            ViewData["categories"] = categories.ResultObject;
            ViewData["products"] = products.ResultObject.Items;
            ViewData["images"] = images.ResultObject;
            if (TempData["result"] != null)
            {
                ViewBag.result = TempData["result"];
                ViewBag.IsSuccess = TempData["IsSuccess"];
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductImageCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var result = await _imageProductServive.AddImage(request.productId,request);
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
                return Redirect($"/product/{request.productId}/images");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int productId,int imageId)
        {

            var result = await _imageProductServive.RemoveImage(productId,imageId);
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
            return Redirect($"/product/{productId}/images");
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ProductImageUpdateRequest request, int productId, int imageId)
        {

            if (ModelState.IsValid)
            {
                var result = await _imageProductServive.UpdateImage(productId,imageId,request);
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
                return Redirect($"/product/{productId}/images");
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }
    }
}