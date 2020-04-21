using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Categorys
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<bool> Create(CategoryCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.LanguageId), "languageId");
            form.Add(new StringContent(request.Name), "Name");
            form.Add(new StringContent(request.IsShowOnHome.ToString()), "IsShowOnHome");
            byte[] data;
            using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
            ByteArrayContent bytes = new ByteArrayContent(data);
            form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            var response = await _client.PostAsync($"/api/categories", form);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int categorytId)
        {
            var response = await _client.DeleteAsync($"/api/categories/{categorytId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId, int pageIndex=0, int pageSize=0)
        {
            var response = await _client.GetAsync($"/api/categories/{languageId}?pageIndex={pageIndex}&pageSize={pageSize}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    return  JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<CategoryViewModel> GetById(int categoryId, string languageId)
        {
            var response = await _client.GetAsync($"/api/categories/{categoryId}/{languageId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    return JsonConvert.DeserializeObject<CategoryViewModel>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> Update(CategoryUpdateRequest request,int categoryId)
        {
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.LanguageId), "languageId");
            form.Add(new StringContent(request.Name), "Name");
            form.Add(new StringContent(request.IsShowOnHome.ToString()), "IsShowOnHome");
            byte[] data;
            using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
            ByteArrayContent bytes = new ByteArrayContent(data);
            form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            var response = await _client.PutAsync($"/api/categories/{categoryId}", form);
            return response.IsSuccessStatusCode;
        }

        public Task<bool> Updatestatus(int CategoryId, CategoryStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
