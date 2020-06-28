using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eShopSolution.WebApp.Models;
using eShopSolution.WebApp.Services.Languages;
using eShopSolution.WebApp.Services.Categorys;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using eShopSolution.WebApp.Services.products;

namespace eShopSolution.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            ILanguageService languageService, 
            ICategoryService categoryService,
            IProductService productService,
            IConfiguration configuration) : base(configuration)
        {
            _languageService = languageService;
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetTopSelling(languageDefauleId, 4);
            var categories = await _categoryService.GetAll("vn",1,6);
            var languages = await _languageService.GetAll();
            ViewData["languages"] = languages.ResultObject;
            ViewData["categories"] = categories.ResultObject;
            ViewData["products"] = products.ResultObject.Items;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
