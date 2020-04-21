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

namespace eShopSolution.AdminApp.Service.ImageProducts
{
    public class ImageProductService : IImageProductService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        public ImageProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
        }
        public async Task<bool> AddImage(int ProductId, ProductImageCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.Caption), "Caption");
            form.Add(new StringContent(request.IsDefault.ToString()), "IsDefault");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }

            var response = await _client.PostAsync($"/api/products/{ProductId}/images", form);
            return response.IsSuccessStatusCode;
        }

        public Task<ProductImageViewModel> GetImageById(int imageId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int ProductId)
        {
            //[HttpGet("{productId}/images")]
            var response = await _client.GetAsync($"/api/products/{ProductId}/Images");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    return JsonConvert.DeserializeObject<List<ProductImageViewModel>>(data);

                }
                else
                {
                    return null;
                }
            }

        }

        public async Task<bool> RemoveImage(int productId,int ImageId)
        {
            //{productId}/images/{imageId}
            var response = await _client.DeleteAsync($"/api/products/images/{ImageId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateImage(int productId,int imageId, ProductImageUpdateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.Caption), "Caption");
            form.Add(new StringContent(request.IsDefault.ToString()), "IsDefault");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }

            var response = await _client.PatchAsync($"/api/products/images/{imageId}", form);
            return response.IsSuccessStatusCode;
        }

    }
}
