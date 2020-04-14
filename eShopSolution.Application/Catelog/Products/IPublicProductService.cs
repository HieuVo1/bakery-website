
using System.Threading.Tasks;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Catalog.Products.Public;

namespace eShopSolution.Application.Catelog.Products
{
    public interface IPublicProductService
    {
        public Task<PageViewModel<ProductViewModel>> GetAllByCategoryId(GetProductPaggingRequest request);
    }
}
