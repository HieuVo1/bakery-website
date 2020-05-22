using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.ReViews
{
    public interface IReviewService
    {
        Task<ApiResult<ReviewViewModel>> GetById(int reviewId);
        Task<ApiResult<string>> Create(ReviewCreateRequest request);
        Task<ApiResult<string>> Update(ReviewUpdateRequest request, int reviewId);
        Task<ApiResult<string>> Delete(int reviewId);
        Task<ApiResult<List<ReviewViewModel>>> GetAll(int productId, int pageIndex = 0, int pageSize = 0);
    }
}
