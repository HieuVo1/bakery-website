
using System.Collections.Generic;
using System.Threading.Tasks;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;

namespace eShopSolution.Application.Catelog.Products
{
    public interface IPublicProductService
    {
        public Task<PageViewModel<ProductViewModel>> GetAllByCategoryId(GetProductPublicPaggingRequest request,int LanguageId);
        public Task<List<ProductViewModel>> GetAll(int languageId);
    }
}
