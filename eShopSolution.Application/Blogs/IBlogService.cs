using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Blogs
{
    public interface IBlogService
    {
        Task<ApiResult<BlogViewModel>> GetById(int blogId);
        Task<ApiResult<bool>> Create(BlogCreateRequest request);
        Task<ApiResult<bool>> Update(BlogUpdateRequest request, int blogId);
        Task<ApiResult<bool>> Liked(int blogId);
        Task<ApiResult<bool>> DisLike(int blogId);
        Task<ApiResult<bool>> Delete(int blogId);
        Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(GetBlogPaggingRequest request);
    }
}
