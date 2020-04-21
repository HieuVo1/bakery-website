
using eShopSolution.ViewModel.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Languages
{
    public interface ILanguageService
    {
        Task<LanguageViewModel> GetById(string languageId);
        Task<List<LanguageViewModel>> GetAll();
    }
}
