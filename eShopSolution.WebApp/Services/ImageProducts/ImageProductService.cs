using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.ImageProducts
{
    public class ImageProductService :BaseService, IImageProductService
    {
        public ImageProductService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
             IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int ProductId)
        {
            return await GetAsync<ApiResult<List<ProductImageViewModel>>>($"/api/productimages/{ProductId}/Images");
        }
    }
}
