using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.products
{
    public class ProductService :BaseService, IProductService
    {
        public ProductService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetAll(string languageId, string Keyword, int pageIndex, int pageSize,int minPrice,int maxPrice)
        {
            return await GetAsync<ApiResult<PageViewModel<ProductViewModel>>>($"/api/products/{languageId}?pageIndex={pageIndex}&pageSize={pageSize}&Keyword={Keyword}&minPrice={minPrice}&maxPrice={maxPrice}");
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetByCategoryUrl(string languageId,string categoryUrl, int pageIndex, int pageSize)
        {
            return await GetAsync<ApiResult<PageViewModel<ProductViewModel>>>($"/api/products/getByUrl/{languageId}?categoryUrl={categoryUrl}&pageIndex={pageIndex}&pageSize={pageSize}");
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId)
        {
            return await GetAsync<ApiResult<ProductViewModel>>($"/api/products/{productId}/{languageId}");
           
        }
        public Task<ApiResult<PageViewModel<ProductViewModel>>> GetByPrice(string languageId, int fromPrice, int toPrice, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetTopSelling(string languageId, int pageSize = 0)
        {
            return await GetAsync<ApiResult<PageViewModel<ProductViewModel>>>($"/api/products/top/{languageId}?pageSize={pageSize}");
        }
    }
}
