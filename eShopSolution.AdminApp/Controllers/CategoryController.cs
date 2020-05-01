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
    public class CategoryController : BaseController
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
            var result = await _languageService.GetAll();
            var indexVN = result.ResultObject.FindIndex(x => x.Name == "VIETNAM");
            if (indexVN != -1)
            {
                SwapGeneric<LanguageViewModel>.Swap(result.ResultObject, indexVN, 0);
            }
            if (TempData["result"] != null)
            {
                ViewBag.result = TempData["result"];
                ViewBag.IsSuccess = TempData["IsSuccess"];
            }
            ViewData["categories"] = categories.ResultObject;
            ViewData["languages"] = result.ResultObject;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                
                request.LanguageId = "vn";
                var result = await _categoryService.Create(request);
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
                return RedirectToAction("Index", "Category", new { id = "vn" });
            }
            else
            {
                return View(request);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.Delete(id);
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
            return RedirectToAction("Index", "Category", new { id = "vn" });
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateRequest request, [FromRoute]int Id)
        {

            if (ModelState.IsValid)
            {
                request.LanguageId = "vn";
                var result = await _categoryService.Update(request, Id);
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
                return RedirectToAction("Index", "Category", new { id = "vn" });
            }
            else
            {
                return View(ModelState.ErrorCount);
            }
        }
    }
}