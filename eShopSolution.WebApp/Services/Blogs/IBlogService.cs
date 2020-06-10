using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Blogs
{
    public interface IBlogService
    {
        Task<ApiResult<BlogViewModel>> GetById(int blogId);
        Task<ApiResult<string>> Liked(LikeCreateRequest request);
        Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(int pageIndex = 0, int pageSize = 0,string categoryUrl=null,string keyword=null, string languageId="vn",string UserId=null);
    }
}
