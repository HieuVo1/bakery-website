using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Email;
using eShopSolution.ViewModel.System.Users;
using eShopSolution.WebApp.Services.Emails;
using eShopSolution.WebApp.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly IEmailService _emailService;
        public UserController(IUserAPIClient userAPIClient,
            IConfiguration configuration,
            SignInManager<UserApp> signInManager,
            IEmailService emailService):base(configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(string Email)
        {
            var result = await _userAPIClient.GetPasswordResetToken(Email);
            if (result.IsSuccessed)
            {
                var verifycatioCode = VerificationCode.GetCode();
                TempData["verifycatioCode"] = verifycatioCode;
                var message = new EmailMessage
                {
                    To = Email,
                    Subject = "Verifition Code",
                    Content = verifycatioCode.ToString()
                };
                var sent = await _emailService.SendEmail(message);
                if (sent.IsSuccessed == true)
                {
                    ViewBag.Email = Email;
                    ViewBag.Token = result.ResultObject;
                    return View("ResetPassword");
                }
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (model.VerificationCode != TempData["verifycatioCode"].ToString())
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

    }
    
}