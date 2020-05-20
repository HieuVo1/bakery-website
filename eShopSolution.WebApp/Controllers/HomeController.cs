using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eShopSolution.WebApp.Models;
using eShopSolution.WebApp.Services.Languages;
using eShopSolution.WebApp.Services.Categorys;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using eShopSolution.WebApp.Helpers;

namespace eShopSolution.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            ILanguageService languageService, 
            ICategoryService categoryService,
            IConfiguration configuration) : base(configuration)
        {
            _languageService = languageService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAll("vn",1,6);
            var languages = await _languageService.GetAll();
            ViewData["languages"] = languages.ResultObject;
            ViewData["categories"] = categories.ResultObject;
            if (section != null)
            {
                ViewBag.IsLogged = true;
            }
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
