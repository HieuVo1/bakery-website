using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Review;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly EShopDbContext _context;
        public ReviewService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(ReviewCreateRequest request)
        {
            var review = new Review()
            {
                Created_At = DateTime.Now,
                Content = request.Content,
                Email = request.Email,
                Rating = request.Rating,
                ProductId = request.ProductId,
                Name = request.Name
            };
            _context.ReViews.Add(review);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Delete(int reviewId)
        {
            var review = await _context.ReViews.FindAsync(reviewId);
            if (review == null) return new ApiResultErrors<bool>($"Can not find review with id: {reviewId}");
            _context.ReViews.Remove(review);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<List<ReviewViewModel>>> GetAll(int productId)
        {
            var query = from p in _context.ReViews
                        where p.ProductId == productId
                        join us in _context.Users on p.Email equals us.Email
                        into usNull
                        from m in usNull.DefaultIfEmpty()
                        select new { p, m };
            int totalRow = await query.CountAsync();
            if (totalRow > 0)
            {
                var data = await query
                 .Select(x => new ReviewViewModel()
                 {
                     Id = x.p.Id,
                     Content = x.p.Content,
                     Created_At = x.p.Created_At,
                     Email = x.p.Email,
                     Name = x.p.Name,
                     Rating = x.p.Rating,
                     ImagePath = x.m.ImagePath,
                 }).ToListAsync();
                return new ApiResultSuccess<List<ReviewViewModel>>(data);
            }
            return new ApiResultErrors<List<ReviewViewModel>>("Not found");
        }

        public Task<ApiResult<ReviewViewModel>> GetById(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> Update(ReviewUpdateRequest request, int reviewId)
        {
            var comment = await _context.ReViews.FindAsync(reviewId);
            comment.Content = request.Content;
            comment.Created_At = DateTime.Now;
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
    }
}
