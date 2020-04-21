using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Products
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

        public async Task<bool> Create(ProductCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
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
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int productId)
        {
            var response = await _client.DeleteAsync($"/api/products/{productId}");
            return response.IsSuccessStatusCode;
        }

        public Task<PageViewModel<ProductViewModel>> GetAllByCategoryId(GetProductPublicPaggingRequest request, string LanguageId)
        {
            throw new NotImplementedException();
        }

        public async Task<PageViewModel<ProductViewModel>> GetAllByCategoryUrl(string categoryUrl, string languageId)
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
                    return JsonConvert.DeserializeObject<PageViewModel<ProductViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<PageViewModel<ProductViewModel>> getAllPagging(GetProductManagePaggingRequest request)
        {
            //https://locahost:port/products/?PageIndex=1&pageSize=1&categoryId=1
            var response = await _client.GetAsync($"/api/products/{request.languageId}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    return JsonConvert.DeserializeObject<PageViewModel<ProductViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ProductViewModel> GetById(int productId,string languageId)
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
                    return JsonConvert.DeserializeObject<ProductViewModel>(data);

                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<bool> Update(ProductUpdateRequest request)
        {
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
            return response.IsSuccessStatusCode;
        }

  

        public Task<bool> UpdatePrice(int ProductId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int ProductId, int addedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
