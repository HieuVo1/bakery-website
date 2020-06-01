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
    public interface IProductService
    {
        Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllPagging(GetProductPaggingRequest request);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryUrl(GetProductPaggingRequest request, string LanguageId);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetTopSelling(GetProductPaggingRequest request);
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<bool>> Delete(int ProductId);
        Task<ApiResult<bool>> UpdatePrice(int ProductId, decimal newPrice);
        Task<ApiResult<bool>> UpdateStock(int ProductId, int addedQuantity);
        Task AddViewCount(int ProductId); 
       
        
    }
}
