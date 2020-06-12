using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using System.Threading.Tasks;

namespace eShopSolution.Application.Blogs
{
    public interface IBlogService
    {
        Task<ApiResult<PageViewModel<BlogViewModel>>> GetById(int blogId);
        Task<ApiResult<bool>> Create(BlogCreateRequest request);
        Task<ApiResult<bool>> Update(BlogUpdateRequest request, int blogId);
        Task<ApiResult<bool>> Like(LikeCreateRequest request);
        Task<ApiResult<bool>> DisLike(LikeCreateRequest request);
        Task<ApiResult<bool>> Delete(int blogId);
        Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(GetBlogPaggingRequest request);
    }
}
