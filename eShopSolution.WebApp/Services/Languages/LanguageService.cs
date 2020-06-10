using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Languages
{
    public class LanguageService : BaseService,ILanguageService
    {
        public LanguageService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) :base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<LanguageViewModel>>>("/api/languages");
        }

        public async Task<ApiResult<LanguageViewModel>> GetById(string languageId)
        {
            return await GetAsync<ApiResult<LanguageViewModel>>($"/api/languages/{languageId}");
        }

    }
}
