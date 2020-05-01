using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.products
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetAll(string languageId, string Keyword, int pageIndex, int pageSize,int minPrice,int maxPrice)
        {
            //https://locahost:port/products/languageId/?PageIndex=1&pageSize=1&categoryUrl=1
            var response = await _client.GetAsync($"/api/products/{languageId}?pageIndex={pageIndex}&pageSize={pageSize}&Keywork={Keyword}&minPrice={minPrice}&maxPrice={maxPrice}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<PageViewModel<ProductViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<PageViewModel<ProductViewModel>>>(data);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetByCategoryUrl(string languageId,string categoryUrl, int pageIndex, int pageSize)
        {
            //https://locahost:port/products/languageId/?PageIndex=1&pageSize=1&categoryUrl=1
            var response = await _client.GetAsync($"/api/products/getByUrl/{languageId}?categoryUrl={categoryUrl}&pageIndex={pageIndex}&pageSize={pageSize}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<PageViewModel<ProductViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<PageViewModel<ProductViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId, int languageId)
        {
            //https://locahost:port/products/productId/languageId
            var response = await _client.GetAsync($"/api/products/{productId}/{languageId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<ProductViewModel>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<ProductViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }
        public Task<ApiResult<PageViewModel<ProductViewModel>>> GetByPrice(string languageId, int fromPrice, int toPrice, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
