using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eShopSolution.AdminApp.Models;
using eShopSolution.AdminApp.Service.Categorys;
using Microsoft.AspNetCore.Http;
using eShopSolution.AdminApp.Service.Languages;
using Microsoft.Extensions.Configuration;
using eShopSolution.AdminApp.Service.Products;

namespace eShopSolution.AdminApp.Controllers
{

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, 
            ICategoryService categoryService,
            ILanguageService languageService,
            IProductService productService,
            IConfiguration configuration) : base(languageService, categoryService, configuration)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult>Index()
        {
            var products = await _productService.GetTopSelling(languageDefauleId, 4);
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
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
