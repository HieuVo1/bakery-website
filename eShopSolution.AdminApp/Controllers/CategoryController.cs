using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Language;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        public CategoryController(ICategoryService categoryService, ILanguageService languageService)
        {
            _categoryService = categoryService;
            _languageService = languageService;
        }
        public async Task<IActionResult> Index([FromRoute] string id)
        {
            var categories = await _categoryService.GetAll(id);
            var languages = await _languageService.GetAll();
            var indexVN = languages.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != 0)
            {
                SwapGeneric<LanguageViewModel>.Swap(languages, indexVN, 0);
            }
            ViewData["categories"] = categories;
            ViewData["languages"] = languages;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                request.LanguageId = "vn";
                var category = await _categoryService.Create(request);

                return RedirectToAction("Index", "category",new { id="vn"});
            }
            else
            {
                return View(request);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var IsDeleted = await _categoryService.Delete(id);
            if (IsDeleted == true) return RedirectToAction("Index", "category", new { id = "vn" });
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateRequest request, [FromRoute]int Id)
        {

            if (ModelState.IsValid)
            {
                request.LanguageId = "vn";
                var isCreated = await _categoryService.Update(request, Id);
                if (isCreated == true)
                {
                    return RedirectToAction("Index", "Category", new { id = "vn" });
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