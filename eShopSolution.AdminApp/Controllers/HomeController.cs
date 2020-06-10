using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eShopSolution.AdminApp.Models;
using eShopSolution.AdminApp.Service.Categorys;
using Microsoft.AspNetCore.Http;
using eShopSolution.AdminApp.Service.Languages;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, 
            ICategoryService categoryService,
            ILanguageService languageService,
            IConfiguration configuration) : base(languageService, categoryService, configuration)
        {
            _logger = logger;
        }

        public async Task<IActionResult>Index()
        {
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);       
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
