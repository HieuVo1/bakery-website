using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Comments;
using eShopSolution.ViewModel.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        //https://locahost:port/api/comments/?PageIndex=1&pageSize=1
        [HttpGet("getAll/{blogId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagging(int blogId)
        {
            var result = await _commentService.GetAll(blogId);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://locahost:port/comment/id
        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int commentId)
        {
            var result = await _commentService.GetById(commentId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _commentService.Create(request);

            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{commentId}")]
        public async Task<IActionResult> Update(CommentUpdateRequest request, int commentId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _commentService.Update(request, commentId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var result = await _commentService.Delete(commentId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
    }
}