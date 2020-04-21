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
        Task<List<ProductImageViewModel>> GetListImage(int ProductId);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<bool> AddImage(int ProductId, ProductImageCreateRequest request);

        Task<bool> RemoveImage(int productId,int ImageId);

        Task<bool> UpdateImage(int productId,int imageId, ProductImageUpdateRequest request);

    }
}
