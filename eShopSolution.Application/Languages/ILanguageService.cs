using eShopSolution.ViewModel.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Languages
{
    public interface ILanguageService
    {
        Task<LanguageViewModel> GetById(string languageId);
        Task<Language> Create(LanguageCreateRequest request);
        Task<Language> Update(LanguageUpdateRequest request,string languageId);
        Task<Language> Delete(string languageId);
        Task<List<LanguageViewModel>> GetAll();
    }
}
