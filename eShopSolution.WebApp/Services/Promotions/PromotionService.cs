using eShopSolution.ViewModel.Catalog.Promotions;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Promotions
{
    public class PromotionService :BaseService, IPromotionService
    {
        public PromotionService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration): base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
       
        public async Task<ApiResult<string>> Delete(int promotionId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/promotions/{promotionId}");
        }

        
        public async Task<ApiResult<PromotionViewModel>> GetByCode(string code)
        {
            return await GetAsync<ApiResult<PromotionViewModel>>($"/api/promotions/getcode/{code}");
        }
    }
}
