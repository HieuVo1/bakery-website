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

    public class ImageProductController : Controller
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
            var paggingRequest = new GetProductManagePaggingRequest()
            {
                languageId = "vn",
            };
            var products = await _productServive.getAllPagging(paggingRequest);
            var productSelected = products.Items.FindIndex(x => x.Id == productId);
            if (productSelected != 0)
            {
                SwapGeneric<ProductViewModel>.Swap(products.Items, productSelected, 0);
            }
            ViewData["categories"] = categories;
            ViewData["products"] = products.Items;
            ViewData["images"] = await _imageProductServive.GetListImage(productId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductImageCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var isCreated = await _imageProductServive.AddImage(request.productId,request);
                if (isCreated == true)
                {
                    return Redirect($"/product/{request.productId}/images");
                }
                return BadRequest();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int productId,int imageId)
        {

            var IsDeleted = await _imageProductServive.RemoveImage(productId,imageId);
            if (IsDeleted == true) return Redirect($"/product/{productId}/images");
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ProductImageUpdateRequest request, int productId, int imageId)
        {

            if (ModelState.IsValid)
            {
                var isUpdated = await _imageProductServive.UpdateImage(productId,imageId,request);
                if (isUpdated == true)
                {
                    return Redirect($"/product/{productId}/images");
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