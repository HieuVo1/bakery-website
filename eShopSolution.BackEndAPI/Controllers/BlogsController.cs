using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Blogs;
using eShopSolution.ViewModel.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        //https://locahost:port/api/blogs/?PageIndex=1&pageSize=1
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagging([FromQuery] GetBlogPaggingRequest request)
        {
            var result = await _blogService.GetAll(request);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://locahost:port/blog/id
        [HttpGet("{blogId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int blogId)
        {
            var result = await _blogService.GetById(blogId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _blogService.Create(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{blogId}")]
        public async Task<IActionResult> Update([FromForm] BlogUpdateRequest request, int blogId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _blogService.Update(request, blogId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{blodId}/like")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateLike(int blodId)
        {
            var result = await _blogService.Liked(blodId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("{blodId}/dislike")]
        [AllowAnonymous]
        public async Task<IActionResult> DisLike(int blodId)
        {
            var result = await _blogService.DisLike(blodId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{blodId}")]
        public async Task<IActionResult> Delete(int blodId)
        {
            var result = await _blogService.Delete(blodId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
    }
}