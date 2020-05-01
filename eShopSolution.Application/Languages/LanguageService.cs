using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModel.Common;
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
        public async Task<ApiResult<bool>> Create(LanguageCreateRequest request)
        {
            var language = new Language()
            {
                Id=request.Id,
                Name = request.Name,
                IsDefault=request.IsDefault
            };
            _context.Languages.Add(language);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Delete(string languageId)
        {
            var language = await _context.Languages.FindAsync(languageId);
            if (language == null) return new ApiResultErrors<bool>($"Can not find language with Id: {languageId}");
            _context.Remove(language);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);

        }

        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            var query = from l in _context.Languages
                            select l;

            var data = await query.Select(x => new LanguageViewModel() { 
                Name= x.Name,
                IsDefault = x.IsDefault,
                Id=x.Id
            }).ToListAsync();

            return new ApiResultSuccess<List<LanguageViewModel>>(data);
        }

        public async Task<ApiResult<LanguageViewModel>> GetById(string languageId)
        {
            var language = await _context.Languages.FindAsync(languageId);
            var languageViewModel = new LanguageViewModel()
            {
                Id=languageId,
                Name=language.Name,
                IsDefault=language.IsDefault
            };
            return new ApiResultSuccess<LanguageViewModel>(languageViewModel);
        }

        public async Task<ApiResult<bool>> Update(LanguageUpdateRequest request,string languageId)
        {
            var language = await _context.Languages.FindAsync(languageId);
            if (language == null) return new ApiResultErrors<bool>($"Can not find language with Id: {languageId}");
            language.Name = request.Name;
            language.IsDefault = request.IsDefault;
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
    }
}
