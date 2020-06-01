using eShopSolution.ViewModel.Catalog.Promotions;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Promotions
{
    public interface IPromotionService
    {
        Task<ApiResult<PromotionViewModel>> GetByCode(string code);
        Task<ApiResult<PromotionViewModel>> GetById(int promotionId);
        Task<ApiResult<string>> Create(PromotionCreateRequest request);
        Task<ApiResult<string>> Update(PromotionUpdateRequest request, int promotionId);
        Task<ApiResult<string>> Delete(int promotionId);
        Task<ApiResult<List<PromotionViewModel>>> GetAll();
    }
}
