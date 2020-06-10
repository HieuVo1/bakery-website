using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.AdminApp.Service.Promotions;
using eShopSolution.ViewModel.Catalog.Promotions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _promotionServive;

        public PromotionController(IPromotionService promotionService,
            ILanguageService languageService,
            ICategoryService categoryService,
            IConfiguration configuration) : base(languageService, categoryService, configuration)
        {
            _promotionServive = promotionService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var result = await _promotionServive.GetAll();
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            if (result.IsSuccessed)
            {
                ViewData["promotions"] = result.ResultObject;
            }
            if (TempData["result"] != null)
            {
                ViewBag.result = TempData["result"];
                ViewBag.IsSuccess = TempData["IsSuccess"];
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PromotionCreateRequest request)
        {

            if (ModelState.IsValid)
            {
                var result = await _promotionServive.Create(request);
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
                return RedirectToAction("Index", "promotion");
            }
            else
            {

                TempData["IsSuccess"] = false;
                TempData["result"] = string.Join(" | ", ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage));
                //ViewBag.IsSuccess = false;
                return RedirectToAction("Index", "promotion");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int promotionId)
        {
            var result = await _promotionServive.Delete(promotionId);
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
            return RedirectToAction("Index", "promotion");
        }
        [HttpPost]
        public async Task<IActionResult> Update(PromotionUpdateRequest request, [FromRoute]int id)
        {

            if (ModelState.IsValid)
            {
                var result = await _promotionServive.Update(request, id);
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
                return RedirectToAction("Index", "promotion");
            }
            else
            {
                TempData["IsSuccess"] = false;
                TempData["result"]= string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return RedirectToAction("Index", "promotion");
            }
        }
    }
}