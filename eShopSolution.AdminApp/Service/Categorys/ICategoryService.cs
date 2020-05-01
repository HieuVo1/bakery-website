using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Categorys
{
    public interface ICategoryService
    {
        Task<ApiResult<CategoryViewModel>> GetById(int categoryId, string languageId);
        Task<ApiResult<string>> Create(CategoryCreateRequest request);
        Task<ApiResult<string>> Update(CategoryUpdateRequest request,int categoriesId);
        Task<ApiResult<string>> Delete(int categorytId);
        Task<ApiResult<string>> Updatestatus(int CategoryId, CategoryStatus status);
        Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId, int pageIndex=0, int pageSize=0);
    }
}
