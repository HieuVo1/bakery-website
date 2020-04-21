using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Categories
{
    public interface IManageCategoryService
    {
        Task<CategoryViewModel> GetById(int categoryId, string languageId);
        Task<Category> Create(CategoryCreateRequest request);
        Task<Category> Update(CategoryUpdateRequest request,int categoryId);
        Task<Category> Delete(int categorytId);
        Task<bool> Updatestatus(int CategoryId, CategoryStatus status);
        Task<List<CategoryViewModel>> GetAll(string languageId);
    }
}
