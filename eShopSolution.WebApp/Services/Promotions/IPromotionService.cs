using eShopSolution.ViewModel.Catalog.Promotions;
using eShopSolution.ViewModel.Common;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Promotions
{
    public interface IPromotionService
    {
        Task<ApiResult<PromotionViewModel>> GetByCode(string code);
        Task<ApiResult<string>> Delete(int promotionId);
    }
}
