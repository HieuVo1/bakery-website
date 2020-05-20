using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.ViewModel.Language;
using eShopSolution.AdminApp.Service.Categorys;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class LanguageController : BaseController
    {
        public LanguageController(ILanguageService languageService,
            ICategoryService categoryService,
            IConfiguration configuration) : base(languageService, categoryService, configuration)
        {
        }
        public async Task<IActionResult> Index()
        {
            ViewData["languages"] = await GetListLanguageAsync();
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            if (TempData["result"] != null)
            {
                ViewBag.result = TempData["result"];
                ViewBag.IsSuccess = TempData["IsSuccess"];
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LanguageCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var result = await _languageService.Create(request);
                if (result.IsSuccessed == true)
                {
                    TempData["result"] = "Create Success";
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["result"] = result.Message;
                    TempData["IsSuccess"] = false;
                }
                return RedirectToAction("Index", "language");
            }
            else
            {
                return BadRequest(ModelState.ErrorCount);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _languageService.Delete(id);
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
            return RedirectToAction("Index", "language");
        }
        [HttpPost]
        public async Task<IActionResult> Update( LanguageUpdateRequest request, [FromRoute]string Id)
        {

            if (ModelState.IsValid)
            {
                var result = await _languageService.Update(request, Id);
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
                return RedirectToAction("Index", "language");
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }
    }
}