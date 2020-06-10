using eShopSolution.ViewModel.Catalog.ProductImages;
using eShopSolution.ViewModel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.ImageProducts
{
    public interface IImageProductService
    {
        Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int ProductId);

    }
}
