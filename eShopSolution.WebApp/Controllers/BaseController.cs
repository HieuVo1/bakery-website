using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShopSolution.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected string section;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.section = context.HttpContext.Session.GetString("Token");
            base.OnActionExecuting(context);
        }
    }
}