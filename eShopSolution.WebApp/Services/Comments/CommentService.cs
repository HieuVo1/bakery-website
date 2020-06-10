using eShopSolution.ViewModel.Comment;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Comments
{
    public class CommentService :BaseService, ICommentService
    {
        public CommentService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<string>> Create(CommentCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>, CommentCreateRequest>($"/api/comments", request);
        }

        public async Task<ApiResult<string>> Delete(int commmentId)
        {
            return await DeleteAsync<ApiResult<string>> ($"/api/comments/{commmentId}");
        }

        public async Task<ApiResult<List<CommentViewModel>>> GetAll(int blogId, int pageIndex = 0, int pageSize = 0)
        {
            return await GetAsync<ApiResult<List<CommentViewModel>>>($"/api/comments/getAll/{blogId}?pageIndex={pageIndex}&pageSize={pageSize}");
            
        }

        public async Task<ApiResult<CommentViewModel>> GetById(int commmentId)
        {
            return await GetAsync<ApiResult<CommentViewModel>>($"/api/comments/{commmentId}");
        }

        public async Task<ApiResult<string>> Update(CommentUpdateRequest request, int commmentId)
        {
            return await UpdateAsync<ApiResult<string>, CommentUpdateRequest>($"/api/comments", request);
        }
    }
}
