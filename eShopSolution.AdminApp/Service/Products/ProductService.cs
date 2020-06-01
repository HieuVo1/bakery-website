using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
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

namespace eShopSolution.AdminApp.Service.Products
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor; 
        private readonly IConfiguration _configuration;
        public ProductService(IHttpClientFactory httpClientFactory,
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

        public async Task<ApiResult<string>> Create(ProductCreateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.LanguageId), "languageId");
            form.Add(new StringContent(request.Name), "Name");
            form.Add(new StringContent(request.Description!=null?request.Description:null), "Description");
            form.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(request.Stock.ToString()), "Stock");
            form.Add(new StringContent(request.Price.ToString()), "Price");
            form.Add(new StringContent(request.OriginalPrice.ToString()), "OriginalPrice");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
           
            var response = await _client.PostAsync($"/api/products", form);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> Delete(int productId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/products/{productId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryId(GetProductPaggingRequest request, string LanguageId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryUrl(string categoryUrl, string languageId)
        {
            //https://locahost:port/products/languageId/?PageIndex=1&pageSize=1&categoryUrl=1
            var response = await _client.GetAsync($"/api/products/getByUrl/{languageId}?categoryUrl={categoryUrl}");
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

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> getAllPagging(GetProductPaggingRequest request)
        {
            //https://locahost:port/products/?PageIndex=1&pageSize=1&categoryId=1
            var response = await _client.GetAsync($"/api/products/{request.LanguageId}");
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

        public async Task<ApiResult<ProductViewModel>> GetById(int productId,string languageId)
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
        public async Task<ApiResult<string>> Update(ProductUpdateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.LanguageId), "languageId");
            form.Add(new StringContent(request.Id.ToString()), "Id");
            form.Add(new StringContent(request.Name), "Name");
            form.Add(new StringContent(request.Description), "Description");
            form.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(request.Stock.ToString()), "Stock");
            form.Add(new StringContent(request.Price.ToString()), "Price");
            form.Add(new StringContent(request.OriginalPrice.ToString()), "OriginalPrice");
            
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            var response = await _client.PatchAsync($"/api/products", form);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

  

        public Task<ApiResult<string>> UpdatePrice(int ProductId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<string>> UpdateStock(int ProductId, int addedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
