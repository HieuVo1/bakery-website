using eShopSolution.Data.EF;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Categories
{
    public class PublicCategoryService : IPublicCategoryService
    {
        private readonly EShopDbContext _context;
        public PublicCategoryService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryViewModel>> GetAll(GetCategoryPaggingReqest request, string languageId)
                                                    {
            var query = from p in _context.Categories
                        join pt in _context.CategoryTranslations on p.Id equals pt.CategoryId
                        where pt.LanguageId == languageId
                        join l in _context.Languages on languageId equals l.Id
                        select new { p, pt, l };
            //filter
            if (!String.IsNullOrEmpty(request.Keywork))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keywork));
            }
            int totalRow = await query.CountAsync();
            //Pagging
            if (request.PageIndex == 0|| request.PageSize==0)
            {
                var data = await query
                .Select(x => new CategoryViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    LanguageId = x.pt.LanguageId,
                    CategoryUrl = x.pt.CategoryUrl,
                    ImagePath = x.p.ImagePath,
                    IsShowOnHome = x.p.IsShowOnHome,
                    Status = x.p.Status,
                    Language = x.l.Name
                }).ToListAsync();
                return data;
            }
            else
            {
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                    .Select(x => new CategoryViewModel()
                    {
                        Id = x.p.Id,
                        Name = x.pt.Name,
                        LanguageId = x.pt.LanguageId,
                        CategoryUrl = x.pt.CategoryUrl,
                        ImagePath = x.p.ImagePath,
                        IsShowOnHome = x.p.IsShowOnHome,
                        Status = x.p.Status,
                        Language = x.l.Name
                    }).ToListAsync();
                return data;
            }

            
        }
    }
}
