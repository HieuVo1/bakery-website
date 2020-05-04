using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Users;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;
        public LoginController(IUserAPIClient userAPIClient, IConfiguration configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("index", "Login");
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request,[FromQuery] string ReturnUrl)
        {
            if (!ModelState.IsValid) return View(ModelState);

            var result = await _userAPIClient.Authenticate(request);
            if (result.IsSuccessed == false)
            {
                TempData["message"] = result.Message;
                ModelState.AddModelError("", result.Message);
                return View();
            }
            TempData["Succes"] = "Login Succsess!";
            HttpContext.Session.SetString("Token", result.ResultObject);

            var userPrincipal = this.ValidateToken(result.ResultObject);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = request.RememberMe // có sử dụng persistent cookie
            };

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    authProperties);

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        [AcceptVerbs("Get", "post")]
        public async Task<IActionResult> IsUserNameUse(string userName)
        {
            var result = await _userAPIClient.GetUserByUserName(userName);
            if (result.IsSuccessed == false)
            {
                return Json(true);
            }
            else
            {
                return Json($"UserName {userName} is already in use");
            }
        }
        [AcceptVerbs("Get", "post")]
        public async Task<IActionResult> IsEmailUse(string Email)
        {
            var result = await _userAPIClient.GetUserByEmail(Email);
            if (result.IsSuccessed == false)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use");
            }
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
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