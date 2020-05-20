using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            //switch (StatusCode)
            //{
            //    case 404:
            //        ViewBag.ErrorMessage = "Sorry, The resource cound not be found";
            //        ViewBag.Path = statusCodeResult.OriginalPath;
            //        ViewBag.QS = statusCodeResult.OriginalQueryString;
            //        break;
            //}
            return View("Error");
        }
    }
}