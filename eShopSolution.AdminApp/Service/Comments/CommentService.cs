using eShopSolution.ViewModel.Comment;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Comments
{
    public class CommentService : ICommentService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;
        public CommentService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<ApiResult<string>> Create(CommentCreateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/comments", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> Delete(int commentId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/comments/{commentId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<List<CommentViewModel>>> GetAll(int blogId)
        {
            var response = await _client.GetAsync($"/api/comments/getAll/{blogId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<List<CommentViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<List<CommentViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<CommentViewModel>> GetById(int commentId)
        {
            var response = await _client.GetAsync($"/api/comments/{commentId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<CommentViewModel>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<CommentViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<string>> Update(CommentUpdateRequest request, int commentId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/api/comments/{commentId}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }
    }
}
