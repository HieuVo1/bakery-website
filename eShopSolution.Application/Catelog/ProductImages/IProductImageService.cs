using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.ProductImages
{
    public interface IProductImageService
    {
        Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int ProductId);
        Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId);
        Task<ApiResult<bool>> AddImage(int ProductId, ProductImageCreateRequest request);
        Task<ApiResult<bool>> RemoveImage(int ImageId);
        Task<ApiResult<bool>> UpdateImage(int imageId, ProductImageUpdateRequest request);
    }
}
