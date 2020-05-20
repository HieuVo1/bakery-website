using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ViewModel.Contact;
using eShopSolution.ViewModel.Email;
using eShopSolution.WebApp.Services.Contacts;
using eShopSolution.WebApp.Services.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IEmailService _emailService;
        private readonly IContactService _contactService;
        public ContactController(IConfiguration configuration,
            IEmailService emailService,
            IContactService contactService) :base(configuration)
        {
            _emailService = emailService;
            _contactService = contactService;
        }
        public IActionResult Index()
        {
            ViewBag.result = TempData["result"];
            ViewBag.IsSuccess = TempData["IsSuccess"];
            if (section != null)
            {
                ViewBag.IsLogged = true;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var message = new EmailMessage
                {
                    To = model.Email,
                    Subject = "Thank for Contact",
                    Content = "Thank you for reaching out to me. I really enjoyed my stay in your apartment and will make sure to come back next year.",
                };
                await _emailService.SendEmail(message);
                var result = await _contactService.Create(model);
                if (result.IsSuccessed == true)
                {
                    TempData["result"] = "Thank for Contact";
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["result"] = result.Message;
                    TempData["IsSuccess"] = false;
                }
                return RedirectToAction("Index", "contact");
            }
            return View(model);
        }
    }
}