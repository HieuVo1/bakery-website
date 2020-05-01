using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Categorys
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;
        public CategoryService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<ApiResult<string>> Create(CategoryCreateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.LanguageId), "languageId");
            form.Add(new StringContent(request.Name), "Name");
            form.Add(new StringContent(request.IsShowOnHome.ToString()), "IsShowOnHome");
            byte[] data;
            if (request.ThumbnailImage != null)
            {
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            
            var response = await _client.PostAsync($"/api/categories", form);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<string>> Delete(int categorytId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/categories/{categorytId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId, int pageIndex=0, int pageSize=0)
        {
            var response = await _client.GetAsync($"/api/categories/{languageId}?pageIndex={pageIndex}&pageSize={pageSize}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<List<CategoryViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<List<CategoryViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<CategoryViewModel>> GetById(int categoryId, string languageId)
        {
            var response = await _client.GetAsync($"/api/categories/{categoryId}/{languageId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<CategoryViewModel>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<CategoryViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<string>> Update(CategoryUpdateRequest request,int categoryId)
        {
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.LanguageId), "languageId");
            form.Add(new StringContent(request.Name), "Name");
            form.Add(new StringContent(request.IsShowOnHome.ToString()), "IsShowOnHome");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            
            var response = await _client.PutAsync($"/api/categories/{categoryId}", form);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }

        public Task<ApiResult<string>> Updatestatus(int CategoryId, CategoryStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
