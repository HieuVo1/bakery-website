using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Products
{
    public class ProductService :BaseService, IProductService
    {

        public ProductService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory, httpContextAccessor, configuration)
        {

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
            return await CreateWithImageAsync<ApiResult<string>>($"/api/products", form);
        }

        public async Task<ApiResult<string>> Delete(int productId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/products/{productId}");
        }

        public Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryId(GetProductPaggingRequest request, string LanguageId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryUrl(string categoryUrl, string languageId)
        {
            return await GetAsync<ApiResult<PageViewModel<ProductViewModel>>>($"/api/products/getByUrl/{languageId}?categoryUrl={categoryUrl}");
        }

        public async Task<ApiResult<PageViewModel<ProductViewModel>>> getAllPagging(GetProductPaggingRequest request)
        {
            return await GetAsync<ApiResult<PageViewModel<ProductViewModel>>>($"/api/products/{request.LanguageId}");
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId,string languageId)
        {
            return await GetAsync<ApiResult<ProductViewModel>>($"/api/products/{productId}/{languageId}");
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
            return await UpdateWithImageAsync<ApiResult<string>>($"/api/products", form);
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
