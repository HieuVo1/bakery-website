using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Categorys
{
    public class CategoryService :BaseService, ICategoryService
    {
        public CategoryService(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
      
        public async Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId, int pageIndex, int pageSize)
        {
            return await GetAsync<ApiResult<List<CategoryViewModel>>>($"/api/categories/{languageId}?PageIndex={pageIndex}&pageSize={pageSize}");
        }

        public async Task<ApiResult<CategoryViewModel>> GetById(int categoryId, int languageId)
        {
            return await GetAsync<ApiResult<CategoryViewModel>>($"/api/categories/{categoryId}/{languageId}");
        }

    }
}
