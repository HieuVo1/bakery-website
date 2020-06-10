using eShopSolution.ViewModel.Catalog.Promotions;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Promotions
{
    public class PromotionService :BaseService, IPromotionService
    {
        public PromotionService(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<string>> Create(PromotionCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>, PromotionCreateRequest>($"/api/promotions", request);
        }

        public async Task<ApiResult<string>> Delete(int promotionId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/promotions/{promotionId}");
        }
        public async Task<ApiResult<List<PromotionViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<PromotionViewModel>>>($"/api/promotions/getAll");
        }

        public async Task<ApiResult<PromotionViewModel>> GetById(int promotionId)
        {
            return await GetAsync<ApiResult<PromotionViewModel>>($"/api/promotions/{promotionId}");
        }

        public async Task<ApiResult<PromotionViewModel>> GetByCode(string code)
        {
            return await GetAsync<ApiResult<PromotionViewModel>>($"/api/promotions/getcode/{code}");
        }

        public async Task<ApiResult<string>> Update(PromotionUpdateRequest request, int promotionId)
        {
            return await UpdateAsync<ApiResult<string>, PromotionUpdateRequest>($"/api/promotions/{promotionId}", request);
        }
    }
}
