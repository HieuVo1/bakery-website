using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Products
{
    public interface IProductService
    {
        Task<ApiResult<PageViewModel<ProductViewModel>>> getAllPagging(GetProductPaggingRequest request);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryId(GetProductPaggingRequest request, string LanguageId);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetAllByCategoryUrl(string categoryUrl, string LanguageId);
        Task<ApiResult<ProductViewModel>> GetById(int productId,string languageId);

        Task<ApiResult<string>> Create(ProductCreateRequest request);

        Task<ApiResult<string>> Update(ProductUpdateRequest request);

        Task<ApiResult<string>> Delete(int productId);

        Task<ApiResult<string>> UpdatePrice(int ProductId, decimal newPrice);

        Task<ApiResult<string>> UpdateStock(int ProductId, int addedQuantity);

    }
}
