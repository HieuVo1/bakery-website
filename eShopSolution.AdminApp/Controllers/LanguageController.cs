using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.ViewModel.Language;
using eShopSolution.AdminApp.Service.Categorys;

namespace eShopSolution.AdminApp.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        public LanguageController(ILanguageService languageService,ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _languageService = languageService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAll("vn");
            var languages = await _languageService.GetAll();
            ViewData["languages"] = languages.ResultObject;
            ViewData["categories"] = categories.ResultObject;
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
                return View(ModelState.ErrorCount);
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