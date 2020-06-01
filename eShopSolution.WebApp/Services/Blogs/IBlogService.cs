using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Blogs
{
    public interface IBlogService
    {
        Task<ApiResult<BlogViewModel>> GetById(int blogId);
        Task<ApiResult<string>> Create(BlogCreateRequest request);
        Task<ApiResult<string>> Update(BlogUpdateRequest request, int blogId);
        Task<ApiResult<string>> Delete(int blogId);
        Task<ApiResult<string>> Liked(int blogId);
        Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(int pageIndex = 0, int pageSize = 0,string categoryUrl=null,string keyword=null);
    }
}
