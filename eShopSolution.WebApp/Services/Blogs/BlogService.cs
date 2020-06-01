using eShopSolution.ViewModel.Blog;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private readonly IConfiguration _configuration;
        private HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;
        public BlogService(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection("BackendUrlBase").Value;
            _client.BaseAddress = new Uri(baseUrl);
        }
        public async Task<ApiResult<string>> Create(BlogCreateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
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

            var response = await _client.PostAsync($"/api/blogs", form);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<string>> Delete(int blogId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/blogs/{blogId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<PageViewModel<BlogViewModel>>> GetAll(int pageIndex = 0, int pageSize = 0,string categoryUrl=null, string keyword = null)
        {
            var response = await _client.GetAsync($"/api/blogs?pageIndex={pageIndex}&pageSize={pageSize}&categoryUrl={categoryUrl}&keyword={keyword}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<PageViewModel<BlogViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<PageViewModel<BlogViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<BlogViewModel>> GetById(int blogId)
        {
            var response = await _client.GetAsync($"/api/blogs/{blogId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<BlogViewModel>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<BlogViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<string>> Liked(int blogId)
        {
            var response = await _client.GetAsync($"/api/blogs/{blogId}/like");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<string>> Update(BlogUpdateRequest request, int blogId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
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

            var response = await _client.PatchAsync($"/api/blogs/{blogId}", form);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }
    }
}
