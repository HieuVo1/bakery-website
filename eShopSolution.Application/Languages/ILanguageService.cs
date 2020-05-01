using eShopSolution.ViewModel.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Common;

namespace eShopSolution.Application.Languages
{
    public interface ILanguageService
    {
        Task<ApiResult<LanguageViewModel>> GetById(string languageId);
        Task<ApiResult<bool>> Create(LanguageCreateRequest request);
        Task<ApiResult<bool>> Update(LanguageUpdateRequest request,string languageId);
        Task<ApiResult<bool>> Delete(string languageId);
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
