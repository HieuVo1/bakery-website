using eShopSolution.ViewModel.Catalog.Promotions;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Promotions
{
    public interface IPromotionService
    {
        Task<ApiResult<PromotionViewModel>> GetByCode(string code);
        Task<ApiResult<PromotionViewModel>> GetById(int promotionId);
        Task<ApiResult<bool>> Create(PromotionCreateRequest request);
        Task<ApiResult<bool>> Update(PromotionUpdateRequest request, int promotionId);
        Task<ApiResult<bool>> Delete(int promotionId);
        Task<ApiResult<List<PromotionViewModel>>> GetAll();
    }
}
