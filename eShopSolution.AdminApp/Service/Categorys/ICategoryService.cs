using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Categorys
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetById(int categoryId, string languageId);
        Task<bool> Create(CategoryCreateRequest request);
        Task<bool> Update(CategoryUpdateRequest request,int categoriesId);
        Task<bool> Delete(int categorytId);
        Task<bool> Updatestatus(int CategoryId, CategoryStatus status);
        Task<List<CategoryViewModel>> GetAll(string languageId, int pageIndex=0, int pageSize=0);
    }
}
