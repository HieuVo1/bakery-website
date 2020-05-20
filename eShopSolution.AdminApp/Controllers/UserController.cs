using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.AdminApp.Service.Users;
using eShopSolution.Utilities.functions;
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
    public class UserController : BaseController
    {
        private readonly IUserAPIClient _userAPIClient;
        public  UserController(IUserAPIClient userAPIClient,
            ICategoryService categoryService,
            ILanguageService languageService,
            IConfiguration configuration) : base(languageService, categoryService, configuration)
        {
            _userAPIClient = userAPIClient;
        }
        public async Task<IActionResult> IndexAsync(string keyword= null,int pageIndex=0,int pageSize=0)
        {
            var section = HttpContext.Session.GetString("Token");
            var request = new GetUserPaggingRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            var roles = await _userAPIClient.getListRole();
            var users = await _userAPIClient.getListUser(request);
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            ViewData["roles"] = roles.ResultObject.Items;
            ViewData["users"] = users.ResultObject.Items;
            ViewData["Token"] = section;
            return View();
        }
        
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userAPIClient.Register(request);
                if (result.IsSuccessed == true)
                {
                    TempData["result"] = "Register Success";
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["result"] = result.Message;
                    TempData["IsSuccess"] = false;
                }
                return RedirectToAction("Index", "User");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userAPIClient.Delete(id);
            if (result.IsSuccessed == true)
            {
                TempData["result"] = "Delete Success";
                TempData["IsSuccess"] = true;
            }
            else
            {
                TempData["result"] = result.Message;
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _userAPIClient.getUserById(id);
            return Ok(data.ResultObject);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UserUpdateRequest request, [FromRoute]Guid Id)
        {

            if (ModelState.IsValid)
            {
                var result = await _userAPIClient.Update(Id,request);
                if (result.IsSuccessed == true)
                {
                    TempData["result"] = "Update Success";
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["result"] = result.Message;
                    TempData["IsSuccess"] = false;
                }
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }
        
    }
}