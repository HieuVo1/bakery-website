using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Blogs
{
    public class BlogService :BaseService, IBlogService
    {
        public BlogService(IHttpClientFactory httpClientFactory,
           IHttpContextAccessor httpContextAccessor,
           IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(int pageIndex = 0, int pageSize = 0,string categoryUrl=null, string keyword = null, string languageId = "vn", string UserId = null)
        {
            return await GetAsync<ApiResult<PageViewModel<BlogViewModel>>>($"/api/blogs?pageIndex={pageIndex}&pageSize={pageSize}&categoryUrl={categoryUrl}&keyword={keyword}&languageId={languageId}&UserId={UserId}");
        }
        public async Task<ApiResult<PageViewModel<BlogViewModel>>> GetById(int blogId)
        {
            return await GetAsync<ApiResult<PageViewModel<BlogViewModel>>>($"/api/blogs/{blogId}");
        }
        public async Task<ApiResult<string>> Like(LikeCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>, LikeCreateRequest>("/api/blogs/like", request);
        }
        public async Task<ApiResult<string>> DisLike(LikeCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>, LikeCreateRequest>("/api/blogs/dislike", request);
        }
    }
}
