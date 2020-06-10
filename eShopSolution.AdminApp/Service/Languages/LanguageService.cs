using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Languages
{
    public class LanguageService :BaseService, ILanguageService
    {
        public LanguageService(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<string>> Create(LanguageCreateRequest request)
        {
            return await CreateAsync<ApiResult<string>, LanguageCreateRequest>($"/api/languages", request);
        }

        public async Task<ApiResult<string>> Delete(string languageId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/languages/{languageId}");
        }

        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<LanguageViewModel>>>($"/api/languages");
        }

        public async Task<ApiResult<LanguageViewModel>> GetById(string languageId)
        {
            return await GetAsync<ApiResult<LanguageViewModel>>($"/api/languages/{languageId}");
        }

        public async Task<ApiResult<string>> Update(LanguageUpdateRequest request, string languageId)
        {
            return await UpdateAsync<ApiResult<string>, LanguageUpdateRequest>($"/api/languages/{languageId}", request);
        }
    }
}
