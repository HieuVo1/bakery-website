using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Languages
{
    public interface ILanguageService
    {
        Task<ApiResult<LanguageViewModel>> GetById(string languageId);
        Task<ApiResult<string>> Create(LanguageCreateRequest request);
        Task<ApiResult<string>> Update(LanguageUpdateRequest request,string languageId);
        Task<ApiResult<string>> Delete(string languageId);
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
