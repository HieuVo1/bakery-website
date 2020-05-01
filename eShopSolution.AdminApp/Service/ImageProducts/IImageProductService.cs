using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.ImageProducts
{
    public interface IImageProductService
    {
        Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int ProductId);

        Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId);

        Task<ApiResult<string>> AddImage(int ProductId, ProductImageCreateRequest request);

        Task<ApiResult<string>> RemoveImage(int productId,int ImageId);

        Task<ApiResult<string>> UpdateImage(int productId,int imageId, ProductImageUpdateRequest request);

    }
}
