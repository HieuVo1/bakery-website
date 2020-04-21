using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Products
{
    public interface IProductService
    {
        Task<PageViewModel<ProductViewModel>> getAllPagging(GetProductManagePaggingRequest request);
        public Task<PageViewModel<ProductViewModel>> GetAllByCategoryId(GetProductPublicPaggingRequest request, string LanguageId);
        public Task<PageViewModel<ProductViewModel>> GetAllByCategoryUrl(string categoryUrl, string LanguageId);
        Task<ProductViewModel> GetById(int productId,string languageId);

        Task<bool> Create(ProductCreateRequest request);

        Task<bool> Update(ProductUpdateRequest request);

        Task<bool> Delete(int productId);

        Task<bool> UpdatePrice(int ProductId, decimal newPrice);

        Task<bool> UpdateStock(int ProductId, int addedQuantity);

    }
}
