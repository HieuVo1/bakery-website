using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Reviews
{
    public class ReviewService : BaseService,IReviewService
    {
        public ReviewService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<string>> Create(ReviewCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>, ReviewCreateRequest>($"/api/reviews", request);

        }

        public async Task<ApiResult<string>> Delete(int reviewId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/reviews/{reviewId}");
          
        }

        public async Task<ApiResult<List<ReviewViewModel>>> GetAll(int productId, int pageIndex = 0, int pageSize = 0)
        {
            return await GetAsync<ApiResult<List<ReviewViewModel>>>($"/api/reviews/getAll/{productId}?pageIndex={pageIndex}&pageSize={pageSize}");
        }

        public Task<ApiResult<ReviewViewModel>> GetById(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> Update(ReviewUpdateRequest request, int reviewId)
        {
            return await UpdateAsync<ApiResult<string>,ReviewUpdateRequest>($"/api/reviews/{reviewId}", request);
        }
    }
}
