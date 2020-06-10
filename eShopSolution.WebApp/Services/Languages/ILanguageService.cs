
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Languages
{
    public interface ILanguageService
    {
        Task<ApiResult<LanguageViewModel>> GetById(string languageId);
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
