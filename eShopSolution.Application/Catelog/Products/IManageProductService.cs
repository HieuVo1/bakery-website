using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Catalog.Products.Manage;
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
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int ProductId);
        Task<bool> UpdatePrice(int ProductId, decimal newPrice);
        Task<bool> UpdateStock(int ProductId, int addedQuantity);
        Task AddViewCount(int ProductId); 
        Task<List<ProductViewModel>> GetAll();
        Task<PageViewModel<ProductViewModel>> getAllPagging(GetProductPaggingRequest request);
        Task<int> AddImage(int ProductId, List<IFormFile> files);
        Task<int> RemoveImage(int ProductId);
        Task<int> UpdateImage(string Caption,int imageId,bool IsDefault);
        Task<List<ProductImageViewModel>> GetListImage(int ProductId);
    }
}
