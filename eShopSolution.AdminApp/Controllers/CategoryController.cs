using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.ViewModel.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(ICategoryService categoryService,
            ILanguageService languageService,
            IConfiguration configuration):base(languageService,categoryService, configuration)
        {
        }
        public async Task<IActionResult> Index([FromRoute] string id)
        { 
            ViewData["categories"] = await GetListCategoryAsync(id);
            ViewData["languages"] = await GetListLanguageAsync();
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