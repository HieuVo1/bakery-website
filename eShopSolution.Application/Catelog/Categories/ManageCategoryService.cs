using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.Data.Enums;
using eShopSolution.Utilities.functions;

namespace eShopSolution.Application.Catelog.Categories
{
    public class ManageCategoryService : IManageCategoryService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ManageCategoryService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<Category> Create(CategoryCreateRequest request)
        {
            var category = new Category()
            {
                IsShowOnHome = request.IsShowOnHome,
                Created_At = DateTime.Now,
                CategoryTranslations = new List<CategoryTranslation>()
                {
                    new CategoryTranslation()
                    {
                        Name=request.Name,
                        CategoryUrl= GetUrlByName.converts(request.Name),
                        LanguageId=request.LanguageId
                    }
                },

            };
            //Save Image
            if (request.ThumbnailImage != null)
            {
                category.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> Delete(int categorytId)
        {
            var category = await _context.Categories.FindAsync(categorytId);
            if (category == null) throw new EShopException($"Cannot find  a product:{categorytId}");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var query = from p in _context.Categories
                        join pt in _context.CategoryTranslations on p.Id equals pt.CategoryId
                        join l in _context.Languages on languageId equals l.Id
                        where pt.LanguageId == languageId
                        select new { p, pt,l };
            //Pagging
            var data = await query.Select(x => new CategoryViewModel()
            {
                Id = x.p.Id,
                Name = x.pt.Name,
                LanguageId = x.pt.LanguageId,
                CategoryUrl = x.pt.CategoryUrl,
                ImagePath = x.p.ImagePath,
                IsShowOnHome = x.p.IsShowOnHome,
                Status = x.p.Status,
                Language=x.l.Name
                
            }).ToListAsync();
            return data;
        }

        public async Task<CategoryViewModel> GetById(int categoryId, string languageId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == categoryId
            && x.LanguageId == languageId);
            var language = await _context.Languages.FindAsync(languageId);

            var categoryViewModel = new CategoryViewModel()
            {
                Id = category.Id,
                LanguageId = categoryTranslation.LanguageId,
                Name = categoryTranslation != null ? categoryTranslation.Name : null,
                Created_At = category.Created_At,
                IsShowOnHome = category.IsShowOnHome,
                Status= category.Status,
                ImagePath = category.ImagePath,
                Language=language.Name,
                CategoryUrl = categoryTranslation != null ? categoryTranslation.CategoryUrl : null
            };
            return categoryViewModel;
        }

        public async Task<Category> Update(CategoryUpdateRequest request,int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == categoryId
            && x.LanguageId == request.LanguageId);
            if (category == null || categoryTranslation == null) throw new EShopException($"Cannot find a category with id: {categoryId}");

            categoryTranslation.Name = request.Name;
            categoryTranslation.CategoryUrl = request.CategoryUrl;
            category.IsShowOnHome = request.IsShowOnHome;
            
            //Save Image
            if (request.ThumbnailImage != null)
            {
                category.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }

             await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> Updatestatus(int CategoryId, CategoryStatus status)
        {
            var category = await _context.Categories.FindAsync(CategoryId);
            if (category == null) throw new EShopException($"Cannot find a category with id: {CategoryId}");
            category.Status = status;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
