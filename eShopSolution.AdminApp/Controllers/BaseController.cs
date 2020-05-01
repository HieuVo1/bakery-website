using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.ViewModel.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        //protected readonly ILanguageService _languageService;
        //protected readonly ICategoryService _categoryService;
        //public BaseController(ICategoryService categoryService, ILanguageService languageService)
        //{
        //    _categoryService = categoryService;
        //    _languageService = languageService;
        //}
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var section = context.HttpContext.Session.GetString("Token");
            if (section == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login",null);
            }
            base.OnActionExecuting(context);
        }
    }
}