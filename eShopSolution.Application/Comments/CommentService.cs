using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Comment;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.Comments
{
    public class CommentService : ICommentService
    {
        private readonly EShopDbContext _context;
        public CommentService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(CommentCreateRequest request)
        {
            var comment = new Comment()
            {
                Created_At = DateTime.Now,
                Content = request.Content,
                UserId = request.UserId,
                BlogId  = request.BlogId

            };
            _context.Comments.Add(comment);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Delete(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return new ApiResultErrors<bool>($"Can not find comment with id: {commentId}");
            _context.Comments.Remove(comment);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<List<CommentViewModel>>> GetAll(int blogId)
        {
            var query = from p in _context.Comments where p.BlogId == blogId
                        join us in _context.Users on p.UserId equals us.Id
                        select new { p, us };
            int totalRow = await query.CountAsync();
            if (totalRow > 0)
            {
                var data = await query
                 .Select(x => new CommentViewModel()
                 {
                     Id = x.p.Id,
                     Content = x.p.Content,
                     Created_At = x.p.Created_At,
                     UserName = x.us.UserName,
                     ImagePath = x.us.ImagePath,
                 }).ToListAsync();
                return new ApiResultSuccess<List<CommentViewModel>>(data);
            }
            
            return new ApiResultErrors<List<CommentViewModel>>("Not found");
            
        }

        public Task<ApiResult<CommentViewModel>> GetById(int commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> Update(CommentUpdateRequest request, int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            comment.Content = request.Content;
            comment.Created_At = DateTime.Now;
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
    }
}
