using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.ImageProducts;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.AdminApp.Service.Products;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{

    public class ImageProductController : BaseController
    {
        private readonly IProductService _productServive;
        private readonly IImageProductService _imageProductServive;
        public ImageProductController(IProductService productService,
            ICategoryService categoryService, 
            IImageProductService imageProductServive,
            ILanguageService languageService,
            IConfiguration configuration) : base(languageService, categoryService, configuration)
        {
            _productServive = productService;
            _imageProductServive = imageProductServive;
        }
        public async Task<IActionResult> IndexAsync(int productId)
        {
            var paggingRequest = new GetProductPaggingRequest()
            {
                LanguageId = languageDefauleId,
            };
            var products = await _productServive.getAllPagging(paggingRequest);
            var images = await _imageProductServive.GetListImage(productId);
            var productSelected = products.ResultObject.Items.FindIndex(x => x.Id == productId);
            if (productSelected != -1)
            {
                SwapGeneric<ProductViewModel>.Swap(products.ResultObject.Items, productSelected, 0);
            }
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            ViewData["products"] = products.ResultObject.Items;
            ViewData["images"] = images.ResultObject;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductImageCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var result = await _imageProductServive.AddImage(request.ProductId,request);
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
                return Redirect($"/product/{request.ProductId}/images");
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