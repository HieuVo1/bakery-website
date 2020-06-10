using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Reviews
{
    public interface IReviewService
    {
        Task<ApiResult<ReviewViewModel>> GetById(int reviewId);
        Task<ApiResult<bool>> Create(ReviewCreateRequest request);
        Task<ApiResult<bool>> Update(ReviewUpdateRequest request, int reviewId);
        Task<ApiResult<bool>> Delete(int reviewId);
        Task<ApiResult<List<ReviewViewModel>>> GetAll(int productId);
    }
}
