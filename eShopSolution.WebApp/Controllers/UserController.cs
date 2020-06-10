using System;
using System.Threading.Tasks;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Email;
using eShopSolution.ViewModel.System.Users;
using eShopSolution.WebApp.Helpers;
using eShopSolution.WebApp.Services.Emails;
using eShopSolution.WebApp.Services.Orders;
using eShopSolution.WebApp.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IOrderService _orderService;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly IEmailService _emailService;
        public UserController(IUserAPIClient userAPIClient,
            IConfiguration configuration,
            SignInManager<UserApp> signInManager,
            IOrderService orderService,
            IEmailService emailService):base(configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailService = emailService;
            _orderService = orderService;
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public async Task<IActionResult> ProfileAsync()
        {
            if (ViewBag.UserId != null)
            {
                var result = await _userAPIClient.GetUserById(new Guid(ViewBag.UserId));
                var orderInfo = await _orderService.GetAllByUser(new string(ViewBag.UserId));
                if (result.IsSuccessed&& orderInfo.IsSuccessed)
                {
                    ViewBag.UserInfo = result.ResultObject; 
                    ViewBag.Orders = orderInfo.ResultObject;
                }
                return View();
            }
            return RedirectToAction("index","login");

        }
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(string Email)
        {
            var result = await _userAPIClient.GetPasswordResetToken(Email);
            if (result.IsSuccessed)
            {
                var sent = await SendMailAsync(Email);
                if (sent == true)
                {
                    ViewBag.Email = Email;
                    ViewBag.Token = result.ResultObject;
                    return View("ResetPassword");
                }
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
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
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var veritifiCode = HttpContext.Session.GetObjectFromJson<string>("verifycatioCode");
            if (model.VerificationCode != veritifiCode)
            {
                ModelState.AddModelError(string.Empty, "Verificate code is invalid");
                return View();
            }
            if (ModelState.IsValid)
            {
                var result = await _userAPIClient.ResetPassword(model);
                if (result.IsSuccessed)
                {
                    return RedirectToAction("index", "login");
                }
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (section == null)
            {
                return RedirectToAction("index", "login");
            }
            model.Email = ViewBag.Email;
            if (ModelState.IsValid)
            {
                var result = await _userAPIClient.ChangePassword(model);
                if (result.IsSuccessed)
                {
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInfo([FromForm] UpdateProfile request)
        {
            if (ModelState.IsValid)
            {
                var UserId = new Guid(ViewBag.UserId);
                var result = await _userAPIClient.Update(UserId, request);
                if (result.IsSuccessed)
                {
                    return RedirectToAction("Profile", "user");
                }
                return View(ModelState.ErrorCount);
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }

    }
    
}