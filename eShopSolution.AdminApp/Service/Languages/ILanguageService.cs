using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Languages
{
    public interface ILanguageService
    {
        Task<LanguageViewModel> GetById(string languageId);
        Task<bool> Create(LanguageCreateRequest request);
        Task<bool> Update(LanguageUpdateRequest request,string languageId);
        Task<bool> Delete(string languageId);
        Task<List<LanguageViewModel>> GetAll();
    }
}
