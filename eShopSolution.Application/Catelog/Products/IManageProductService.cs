using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Products
{
    public interface IManageProductService
    {
        Task<ProductViewModel> GetById(int productId, string languageId);
        
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int ProductId);
        Task<bool> UpdatePrice(int ProductId, decimal newPrice);
        Task<bool> UpdateStock(int ProductId, int addedQuantity);
        Task AddViewCount(int ProductId); 
        Task<List<ProductViewModel>> GetAll();
        Task<PageViewModel<ProductViewModel>> getAllPagging(GetProductManagePaggingRequest request);
        public Task<PageViewModel<ProductViewModel>> GetAllByCategoryUrl(GetProductManagePaggingRequest request, string LanguageId);
        Task<List<ProductImageViewModel>> GetListImage(int ProductId);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<int> AddImage(int ProductId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int ImageId);
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);
    }
}
