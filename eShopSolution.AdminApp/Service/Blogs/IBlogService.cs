using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Blogs
{
    public interface IBlogService
    {
        Task<ApiResult<PageViewModel<BlogViewModel>>> GetById(int blogId);
        Task<ApiResult<string>> Create(BlogCreateRequest request);
        Task<ApiResult<string>> Update(BlogUpdateRequest request, int blogId);
        Task<ApiResult<string>> Delete(int blogId);
        Task<ApiResult<string>> Liked(int blogId);
        Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll( int pageIndex = 0, int pageSize = 0,string languageId="vn");
    }
}
