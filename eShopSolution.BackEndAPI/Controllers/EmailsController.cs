using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eShopSolution.Application.Comom;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("SendMessage")]
        public IActionResult ConfirmEmail(EmailMessage messgae)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sentmail = new Thread(() =>
            {
                _emailService.SendEmailAsync(messgae);
            });
            sentmail.Start();
            var result = new ApiResultSuccess<string>();
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}