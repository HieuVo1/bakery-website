using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eShopSolution.AdminApp.Models;
using Microsoft.AspNetCore.Authorization;
using eShopSolution.AdminApp.Service.Categorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace eShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult>Index()
        {
            var categories = await _categoryService.GetAll("vn");
            ViewData["categories"] = categories;
            var user = User.Identity.Name;
            var role = User.Identity.AuthenticationType;
            
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
