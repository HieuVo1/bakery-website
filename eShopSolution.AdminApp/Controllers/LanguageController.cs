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
            ViewData["languages"] = languages;
            ViewData["categories"] = categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LanguageCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var isCreated = await _languageService.Create(request);
                if (isCreated == true) {
                    return RedirectToAction("Index", "language");
                }
                return BadRequest();
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var IsDeleted = await _languageService.Delete(id);
            if (IsDeleted == true) return RedirectToAction("Index", "language");
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Update( LanguageUpdateRequest request, [FromRoute]string Id)
        {

            if (ModelState.IsValid)
            {
                var isCreated = await _languageService.Update(request, Id);
                if (isCreated == true)
                {
                    return RedirectToAction("Index", "language");
                }
                return BadRequest();
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }
    }
}