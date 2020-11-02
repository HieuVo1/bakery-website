using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Blogs
{
    public class BlogService :BaseService, IBlogService
    {
        public BlogService(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<string>> Create(BlogCreateRequest request)
        {
         
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.Content), "Content");
            form.Add(new StringContent(request.Title), "Title");
            form.Add(new StringContent(request.UserId.ToString()), "UserId");
            form.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            byte[] data;
            if (request.ThumbnailImage != null)
            {
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            return await CreateWithImageAsync<ApiResult<string>>($"/api/blogs", form);
        }

        public async Task<ApiResult<string>> Delete(int blogId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/blogs/{blogId}");
        }

        public async Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(int pageIndex = 0, int pageSize = 0,string languageId="vn")
        {
            return await GetAsync<ApiResult<PageViewModel<BlogViewModel>>>($"/api/blogs?pageIndex={pageIndex}&pageSize={pageSize}&languageId={languageId}");
        }

        public async Task<ApiResult<PageViewModel<BlogViewModel>>> GetById(int blogId)
        {
            return await GetAsync<ApiResult<PageViewModel<BlogViewModel>>>($"/api/blogs/{blogId}");
        }

        public async Task<ApiResult<string>> Liked(int blogId)
        {
            return await GetAsync<ApiResult<string>>($"/api/blogs/{blogId}/like");
        }

        public async Task<ApiResult<string>> Update(BlogUpdateRequest request, int blogId)
        {
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.Content), "Content");
            form.Add(new StringContent(request.Title), "Title");
            form.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            return await UpdateWithImageAsync<ApiResult<string>>($"/api/blogs/{blogId}", form);
        }
    }
}
