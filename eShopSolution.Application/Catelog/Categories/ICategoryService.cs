using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Categories
{
    public interface ICategoryService
    {
        Task<ApiResult<CategoryViewModel>> GetById(int categoryId, string languageId);
        Task<ApiResult<bool>> Create(CategoryCreateRequest request);
        Task<ApiResult<bool>> Update(CategoryUpdateRequest request,int categoryId);
        Task<ApiResult<bool>> Delete(int categorytId);
        Task<ApiResult<bool>> Updatestatus(int CategoryId, CategoryStatus status);
        Task<ApiResult<List<CategoryViewModel>>> GetAll(GetCategoryPaggingReqest request, string languageId);
    }
}
