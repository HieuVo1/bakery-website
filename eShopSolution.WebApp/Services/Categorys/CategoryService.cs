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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Categorys
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
      
        public async Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId, int pageIndex, int pageSize)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.GetAsync($"/api/categories/{languageId}?PageIndex={pageIndex}&pageSize={pageSize}");
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

        public async Task<ApiResult<CategoryViewModel>> GetById(int categoryId, int languageId)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.GetAsync($"/api/categories/{categoryId}/{languageId}");
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

    }
}
