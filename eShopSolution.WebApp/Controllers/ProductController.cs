using System.Threading.Tasks;
using eShopSolution.ViewModel.Email;
using eShopSolution.ViewModel.Review;
using eShopSolution.WebApp.Services.Categorys;
using eShopSolution.WebApp.Services.Emails;
using eShopSolution.WebApp.Services.ImageProducts;
using eShopSolution.WebApp.Services.Languages;
using eShopSolution.WebApp.Services.products;
using eShopSolution.WebApp.Services.Reviews;
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
        private readonly IReviewService _reviewService;
        private readonly IEmailService _emailService;
        private readonly int _pageSize = 6;
        public ProductController(IProductService productService, 
            ICategoryService categoryService,
            ILanguageService languageService,
            IImageProductService imageProductService,
            IReviewService reviewService,
            IEmailService emailService,
            IConfiguration configuration):base(configuration)
        {
            _productService = productService;
            _categoryService = categoryService;
            _languageService = languageService;
            _imageProductService = imageProductService;
            _reviewService = reviewService;
            _emailService = emailService;
        }
        public async Task<IActionResult> IndexAsync([FromQuery] int minPrice, int maxPrice,int pageIndex=1,string Name=null)
        
        {
            
            var topSelling = await _productService.GetTopSelling(languageDefauleId, 3);
            var products = await _productService.GetAll("vn", Name, pageIndex, _pageSize, minPrice,maxPrice);
            var categories = await _categoryService.GetAll("vn");
            ViewBag.CategoryUrl = "index";
            ViewData["products"] = products.ResultObject.Items;
            ViewData["minPrice"] = minPrice;
            ViewData["maxPrice"] = maxPrice;
            ViewData["categories"] = categories.ResultObject;
            ViewBag.top = topSelling.ResultObject.Items;
            if (section != null)
            {
                ViewBag.IsLogged = true;
            }
            return View(products.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> GetByURl(string categoryUrl, [FromQuery] int page = 1)
        {
            var products = await _productService.GetByCategoryUrl( "vn", categoryUrl, page,_pageSize);
            var categories = await _categoryService.GetAll("vn");
            ViewBag.CategoryUrl = categoryUrl;
            ViewData["products"] = products.ResultObject.Items;
            ViewData["categories"] = categories.ResultObject;
            ViewData["total"] = products.ResultObject.TotalRecords;
            ViewData["NumPage"] =(products.ResultObject.TotalRecords % _pageSize==0)?products.ResultObject.TotalRecords / _pageSize: products.ResultObject.TotalRecords / _pageSize+1;
            return View("Index", products.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> DetailAsync(int productId)
        {
            var topSelling = await _productService.GetTopSelling(languageDefauleId, 4);
            var result = await _productService.GetById(productId, languageDefauleId);
            var images = await _imageProductService.GetListImage(productId);
            var reviews = await _reviewService.GetAll(productId);
            ViewBag.ListImage = images.ResultObject;
            ViewBag.ListReview = reviews.ResultObject;
            ViewBag.top = topSelling.ResultObject.Items;
            return View(result.ResultObject);
        }
        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _reviewService.Create(request);
                if (result.IsSuccessed)
                {
                    var message = new EmailMessage
                    {
                        To = request.Email,
                        Subject = "Thank for review",
                        Content = "Thank you for reaching out to me. I really enjoyed my stay in your apartment and will make sure to come back next year.",
                    };
                    await _emailService.SendEmail(message);
                    return RedirectToAction("detail", new { productId = request.ProductId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(request);
                }
            }
            return View(request);
        }
    }
}