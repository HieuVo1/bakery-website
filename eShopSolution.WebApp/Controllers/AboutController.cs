using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class AboutController : BaseController
    {
        public AboutController(IConfiguration configuration):base(configuration)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}