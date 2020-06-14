using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ILanguageService _languageService;
        protected readonly ICategoryService _categoryService;
        protected readonly IConfiguration _configuration;
        protected  string languageDefauleId;
        public BaseController(ILanguageService languageService, 
            ICategoryService categoryService,
            IConfiguration configuration)
        {
            _categoryService = categoryService;
            _languageService = languageService;
            _configuration = configuration;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var section = context.HttpContext.Session.GetString("Token");
            if (section == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login",null);
            }
            else {
                var userPrincipal = ValidateToken(section);
                ViewBag.ImagePath = userPrincipal.Claims.FirstOrDefault(c => c.Type == "Picture").Value;
                ViewBag.UserName = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                ViewBag.Email = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                ViewBag.Id = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                ViewBag.Role = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            }
            ViewBag.result = TempData["result"];
             ViewBag.IsSuccess = TempData["IsSuccess"];
             languageDefauleId = _configuration.GetSection("LanguageDefaultId").Value;
            base.OnActionExecuting(context);
        }
        public async Task<List<LanguageViewModel>> GetListLanguageAsync()
        {
            var listLanguage = await _languageService.GetAll();
            var indexVN = listLanguage.ResultObject.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != -1)
            {
                SwapGeneric<LanguageViewModel>.Swap(listLanguage.ResultObject, indexVN, 0);
            }
            return listLanguage.ResultObject;
        }
        public async Task<List<CategoryViewModel>> GetListCategoryAsync(string languageId,string categoryUrl=null)
        {
            var listCategory = await _categoryService.GetAll(languageId);
            if (categoryUrl != null)
            {
                var index = listCategory.ResultObject.FindIndex(x => x.CategoryUrl == categoryUrl);
                if (index != 0)
                {
                    SwapGeneric<CategoryViewModel>.Swap(listCategory.ResultObject, index, 0);
                }
            }
            return listCategory.ResultObject;
        }
        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            var Iss = _configuration["Tokens:Issuer"];
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}