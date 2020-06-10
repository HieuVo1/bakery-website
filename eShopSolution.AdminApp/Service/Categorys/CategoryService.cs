using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Categorys
{
    public class CategoryService :BaseService, ICategoryService
    {
        public CategoryService(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<string>> Create(CategoryCreateRequest request)
        {
           
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
            return await CreateWithImageAsync<ApiResult<string>>($"/api/categories", form);
        }

        public async Task<ApiResult<string>> Delete(int categorytId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/categories/{categorytId}");
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId, int pageIndex=0, int pageSize=0)
        {
            return await GetAsync<ApiResult<List<CategoryViewModel>>>($"/api/categories/{languageId}?pageIndex={pageIndex}&pageSize={pageSize}");
        }

        public async Task<ApiResult<CategoryViewModel>> GetById(int categoryId, string languageId)
        {
            return await GetAsync<ApiResult<CategoryViewModel>>($"/api/categories/{categoryId}/{languageId}");
        }
        public async Task<ApiResult<string>> Update(CategoryUpdateRequest request,int categoryId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
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
            return await UpdateWithImageAsync<ApiResult<string>>($"/api/categories/{categoryId}", form);
        }

        public Task<ApiResult<string>> Updatestatus(int CategoryId, CategoryStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
