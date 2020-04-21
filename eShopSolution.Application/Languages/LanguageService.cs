using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Language;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.Languages
{

    public class LanguageService : ILanguageService
    {
        private readonly EShopDbContext _context;
        public LanguageService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<Language> Create(LanguageCreateRequest request)
        {
            var language = new Language()
            {
                Id=request.Id,
                Name = request.Name,
                IsDefault=request.IsDefault
            };
            _context.Languages.Add(language);
            await _context.SaveChangesAsync();
            return language;
        }

        public async Task<Language> Delete(string languageId)
        {
            var language = await _context.Languages.FindAsync(languageId);
            if (language == null) throw new EShopException($"Can not find language with Id: {languageId}");
            _context.Remove(language);
            await _context.SaveChangesAsync();
            return language;

        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            var query = from l in _context.Languages
                            select l;

            var data = await query.Select(x => new LanguageViewModel() { 
                Name= x.Name,
                IsDefault = x.IsDefault,
                Id=x.Id
            }).ToListAsync();

            return data;
        }

        public async Task<LanguageViewModel> GetById(string languageId)
        {
            var language = await _context.Languages.FindAsync(languageId);
            var languageViewModel = new LanguageViewModel()
            {
                Id=languageId,
                Name=language.Name,
                IsDefault=language.IsDefault
            };
            return languageViewModel;
        }

        public async Task<Language> Update(LanguageUpdateRequest request,string languageId)
        {
            var language = await _context.Languages.FindAsync(languageId);
            if (language == null) throw new EShopException($"Can not find language with Id: {languageId}");
            language.Name = request.Name;
            language.IsDefault = request.IsDefault;
            await _context.SaveChangesAsync();
            return language;
        }
    }
}
