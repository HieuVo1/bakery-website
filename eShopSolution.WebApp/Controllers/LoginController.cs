using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Carts;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Email;
using eShopSolution.ViewModel.System.Users;
using eShopSolution.WebApp.Helpers;
using eShopSolution.WebApp.Services.Emails;
using eShopSolution.WebApp.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
namespace eShopSolution.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly  IEmailService _emailService;
        private readonly ICartService _cartService;
        public LoginController(IUserAPIClient userAPIClient,
            IConfiguration configuration,
            SignInManager<UserApp> signInManager,
            IEmailService emailService,
            ICartService cartService)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailService = emailService;
            _cartService = cartService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            GetCart();
            return View();
        }
        [HttpGet]
        public IActionResult GoogleLogin(string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Login", new
            { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }
        [HttpGet]
        public IActionResult FaceBookLogin(string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Login", new
            { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string remoteError = null)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider:{remoteError}");
                return View("Index");
            }
            var externalLoginRequest = new ExternalLoginRequest
            {
                FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                ProviderKey = info.ProviderKey,
                LoginProvider = info.LoginProvider,
                ProviderDisPlayName = info.ProviderDisplayName,
                ImagePath = info.Principal.Claims.FirstOrDefault(c => c.Type == "picture").Value,
            };
            var result = await _userAPIClient.ExternalLoginCallback(externalLoginRequest);
            if (result.IsSuccessed == false)
            {
                TempData["message"] = result.Message;
                ModelState.AddModelError("", result.Message);
                ViewBag.ErrorServerSide = true;
                return View();
            }
            TempData["Succes"] = "Login Succsess!";
            HttpContext.Session.SetString("Token", result.ResultObject);

            var userPrincipal = this.ValidateToken(result.ResultObject);
            var UserId = new Guid(userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var cartResult = await _cartService.GetById(UserId);
            if (cartResult.IsSuccessed == true)
            {
                var CartSessionKey = _configuration.GetSection("CartSessionKey").Value;
                
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cartResult.ResultObject.CartItems);
            }
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false // có sử dụng persistent cookie
            };

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    authProperties);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            var CartSessionKey = _configuration.GetSection("CartSessionKey").Value;
            HttpContext.Session.Remove(CartSessionKey);
            HttpContext.Session.Remove("CartId");
            HttpContext.Session.Remove("Token");
            return RedirectToAction("index", "Login");
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request, [FromQuery] string ReturnUrl)
        {
            if (!ModelState.IsValid) return View(ModelState);

            var result = await _userAPIClient.Authenticate(request);
            if (result.IsSuccessed == false)
            {
                TempData["message"] = result.Message;
                ModelState.AddModelError("", result.Message);
                ViewBag.ErrorServerSide = true;
                return View();
            }
            TempData["Succes"] = "Login Succsess!";
            HttpContext.Session.SetString("Token", result.ResultObject);
            var userPrincipal = this.ValidateToken(result.ResultObject);
            var UserId = new Guid(userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var cartResult = await _cartService.GetById(UserId);
            if (cartResult.IsSuccessed == true)
            {
                var CartSessionKey = _configuration.GetSection("CartSessionKey").Value;
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cartResult.ResultObject.CartItems);
            }
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true // có sử dụng persistent cookie
            };
            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    authProperties);
           
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userAPIClient.Register(request);
                if (result.IsSuccessed == false)
                {
                    TempData["message"] = result.Message;
                    ModelState.AddModelError("", result.Message);
                    ViewBag.ErrorServerSide = true;
                    return View();
                }
                var sent = await SendMailAsync(request.Email);
                if (sent == true)
                {
                    ViewBag.Email = request.Email;
                    ViewBag.UserId = result.ResultObject.UserId;
                    ViewBag.Token = result.ResultObject.Token;
                    return View("ConfirmEmail");
                }
                ModelState.AddModelError(string.Empty, "Send Verifition code Faild");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }
        public async Task<bool> SendMailAsync(string email)
        {
            var verifycatioCode = VerificationCode.GetCode();
            HttpContext.Session.SetObjectAsJson("verifycatioCode", verifycatioCode);
            var message = new EmailMessage
            {
                To = email,
                Subject = "Verifition Code",
                Content = verifycatioCode.ToString()
            };
            var result = await _emailService.SendEmail(message);
            return result.IsSuccessed;
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(VerificationViewModel verificationViewModel)
        {
            var veritifiCode = HttpContext.Session.GetObjectFromJson<string>("verifycatioCode");
            if (verificationViewModel.VerificationCode != veritifiCode)
            {
                ModelState.AddModelError(string.Empty, "Verificate code is invalid ");
                return RedirectToAction("register", "login");
            }
            if (verificationViewModel.UserId == null && verificationViewModel.Token == null)
            {
                return RedirectToAction("register", "login");
            }
            var result = await _userAPIClient.ConfirmEmail(verificationViewModel);
            if (result.IsSuccessed)
            {
                return RedirectToAction("index", "login");
            }
            ModelState.AddModelError(string.Empty,result.Message);
            return View();
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
        public void GetCart()
        {
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
            var CartSessionKey = _configuration.GetSection("CartSessionKey").Value;
            cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartSessionKey);
            ViewBag.cart = cartItems;
            ViewBag.total = (cartItems != null) ? cartItems.Sum(item => item.Product.Price * item.Quantity) : 0;
            ViewBag.NumItem = (cartItems != null) ? cartItems.Sum(x => x.Quantity) : 0;
        }
    }
}