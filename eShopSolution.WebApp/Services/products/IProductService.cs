using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.products
{
    public interface IProductService
    {
        Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetAll(string languageId, string Keyword, int pageIndex = 0, int pageSize = 0, int minPrice = 0,int maxPrice=0);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetByCategoryUrl(string languageId,string categoryUrl, int pageIndex=0, int pageSize=0);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetByPrice(string languageId,int fromPrice,int toPrice, int pageIndex=0, int pageSize=0);
        Task<ApiResult<PageViewModel<ProductViewModel>>> GetTopSelling(string languageId, int pageSize = 0);
    }
}
