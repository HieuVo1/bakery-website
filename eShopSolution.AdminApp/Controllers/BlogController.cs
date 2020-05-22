using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Blogs;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.Utilities.functions;
using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(ICategoryService categoryService,
            ILanguageService languageService,
            IConfiguration configuration,
            IBlogService blogService):base(languageService, categoryService, configuration)
        {
            _blogService = blogService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var blogs = await _blogService.GetAll();
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            ViewData["blogs"] = blogs.ResultObject.Items;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> PostBlogAsync()
        {
            ViewData["categories"] = await GetListCategoryAsync(languageDefauleId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostBlog(BlogCreateRequest model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = new Guid(ViewBag.Id);
                var result = await _blogService.Create(model);
                if (result.IsSuccessed == true)
                {
                    TempData["result"] = "Add Success";
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["result"] = result.Message;
                    TempData["IsSuccess"] = false;
                }
                return RedirectToAction("Index", "blog");
            }
            
            return View(model);
        }
        public async Task<IActionResult> EditAsync(int blogId)
        {
            var blog = await _blogService.GetById(blogId);
            var categories = await GetListCategoryAsync(languageDefauleId);
            ViewData["blog"] = blog.ResultObject;
            var index = categories.FindIndex(x => x.Name == blog.ResultObject.CategoryName);
            //Swap
            if (index != 0)
            {
                SwapGeneric<CategoryViewModel>.Swap(categories, index, 0);
            }
            ViewData["categories"] = categories;
            return View("PostBlog");
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync([FromForm] BlogUpdateRequest request, [FromRoute]int blogId)
        {
            if (ModelState.IsValid)
            {
                var result = await _blogService.Update(request, blogId);
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
                return RedirectToAction("Index", "blog");
            }
            return View(request);
        }
        public async Task<IActionResult> Delete( int blogId)
        {
            var result = await _blogService.Delete(blogId);
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
            return RedirectToAction("Index", "blog");
        }
    }
}