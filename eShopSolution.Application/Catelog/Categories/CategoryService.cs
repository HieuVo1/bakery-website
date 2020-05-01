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
using eShopSolution.ViewModel.Common;

namespace eShopSolution.Application.Catelog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public CategoryService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<bool>> Create(CategoryCreateRequest request)
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
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Delete(int categorytId)
        {
            var category = await _context.Categories.FindAsync(categorytId);
            if (category == null) return new ApiResultErrors<bool>($"Can not find Category with id: {categorytId}");
            _context.Categories.Remove(category);
            return await SaveChangeService.SaveChangeAsyncImage(_context,category.ImagePath,_storageService);
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll(GetCategoryPaggingReqest request, string languageId)
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
            if (request.PageIndex == 0 || request.PageSize == 0)
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
                return new ApiResultSuccess<List<CategoryViewModel>>(data);
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
                return new ApiResultSuccess<List<CategoryViewModel>>(data);
            }


        }

        public async Task<ApiResult<CategoryViewModel>> GetById(int categoryId, string languageId)
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
            return new ApiResultSuccess<CategoryViewModel>(categoryViewModel);
        }

        public async Task<ApiResult<bool>> Update(CategoryUpdateRequest request,int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == categoryId
            && x.LanguageId == request.LanguageId);
            if (category == null || categoryTranslation == null) return new ApiResultErrors<bool>($"Cannot find a category with id: {categoryId}");

            categoryTranslation.Name = request.Name;
            categoryTranslation.CategoryUrl = GetUrlByName.converts(request.Name);
            category.IsShowOnHome = request.IsShowOnHome;
            
            //Save Image
            if (request.ThumbnailImage != null)
            {
                var OldImagePath = category.ImagePath;
                category.ImagePath = await this.SaveFile(request.ThumbnailImage);
                await _storageService.DeleteFileAsync(OldImagePath);
            }
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> Updatestatus(int CategoryId, CategoryStatus status)
        {
            var category = await _context.Categories.FindAsync(CategoryId);
            if (category == null) return new ApiResultErrors<bool>($"Cannot find a category with id: {CategoryId}");
            category.Status = status;
            try
            {
                var change = await _context.SaveChangesAsync();
                if (change > 0)
                {
                    return new ApiResultSuccess<bool>();
                }
                else
                {
                    return new ApiResultErrors<bool>("Update faild");
                }
            }
            catch (Exception ex)
            {
                return new ApiResultErrors<bool>(ex.InnerException.Message);
            }
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
