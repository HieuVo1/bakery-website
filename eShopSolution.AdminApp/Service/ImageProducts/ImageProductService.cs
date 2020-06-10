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

namespace eShopSolution.AdminApp.Service.ImageProducts
{
    public class ImageProductService :BaseService, IImageProductService
    {
        public ImageProductService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<string>> AddImage(int ProductId, ProductImageCreateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
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
            return await CreateWithImageAsync<ApiResult<string>>($"/api/productimages/{ProductId}/images", form);
        }

        public Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int ProductId)
        {
            return await GetAsync<ApiResult<List<ProductImageViewModel>>>($"/api/productimages/{ProductId}/Images");
           
        }

        public async Task<ApiResult<string>> RemoveImage(int productId,int ImageId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/productimages/images/{ImageId}");
        }

        public async Task<ApiResult<string>> UpdateImage(int productId,int imageId, ProductImageUpdateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
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
            return await UpdateWithImageAsync<ApiResult<string>>($"/api/productimages/images/{imageId}", form);
        }

    }
}
