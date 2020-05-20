using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.WebApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected string section;
        protected IConfiguration _configuration;
        protected string languageDefauleId;
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            section=CookieHelpers.GetObjectFromJson<string>(Request.Cookies,"Token");
            GetCart();
            if (section != null)
            {
                var userPrincipal = ValidateToken(section);
                ViewBag.ImagePath = userPrincipal.Claims.FirstOrDefault(c => c.Type == "Picture").Value;
                ViewBag.UserName = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                ViewBag.Email = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                ViewBag.UserId = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            }
            languageDefauleId = _configuration.GetSection("LanguageDefaultId").Value;
            base.OnActionExecuting(context);
        }
        public void GetCart()
        {
            List<CartItemViewModel> cart = new List<CartItemViewModel>();
            var CartSessionKey = _configuration.GetSection("CartSessionKey").Value;
            cart = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
            ViewBag.cart = cart;
            ViewBag.total = (cart != null) ? cart.Sum(item => item.Product.Price * item.Quantity) : 0;
            ViewBag.NumItem = (cart != null)?cart.Sum(x=>x.Quantity):0;
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