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