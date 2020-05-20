using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ViewModel.Comment;
using eShopSolution.WebApp.Services.Blogs;
using eShopSolution.WebApp.Services.Comments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        private readonly int _pageSize = 5;
        public BlogController(IConfiguration configuration, 
            IBlogService blogService,
            ICommentService commentService):base(configuration)
        {
            _blogService = blogService;
            _commentService = commentService;
        }
        public async Task<IActionResult> IndexAsync([FromQuery] int pageIndex = 1)
        {
            var blogs = await _blogService.GetAll(pageIndex, _pageSize);
            ViewData["blogs"] = blogs.ResultObject.Items;
            if (section != null)
            {
                ViewBag.IsLogged = true;
            }
            return View(blogs.ResultObject);
        }
        public async Task<IActionResult> DetailAsync(int blogId)
        {
            var result = await _blogService.GetById(blogId);
            var comments = await _commentService.GetAll(blogId);
            ViewData["blog"] = result.ResultObject;
            ViewData["comments"] = comments.ResultObject;
            if (section != null)
            {
                ViewBag.IsLogged = true;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CommentAsync(CommentCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                request.UserId = new Guid(ViewBag.UserId);
                var result = await _commentService.Create(request);
                if (result.IsSuccessed == true)
                {
                    return RedirectToAction("detail",new { blogId  = request.BlogId});
                }
                ModelState.AddModelError(string.Empty, result.Message);
                return View(request);
            }
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> EditComment(CommentUpdateRequest request,int commentId)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.Update(request, commentId);
                if (result.IsSuccessed == true)
                {
                    return RedirectToAction("detail", new { blogId = request.BlogId });
                }
                ModelState.AddModelError(string.Empty, result.Message);
                return View(request);
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> deleteCommentAsync(int commentId,int blogId)
        {
            var result = await _commentService.Delete(commentId);
            if (result.IsSuccessed)
            {
                return RedirectToAction("detail", new { blogId = blogId });
            }
            return View();
        }
    }
}