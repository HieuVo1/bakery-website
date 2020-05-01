
using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Categorys
{
    public interface ICategoryService
    {
        Task<ApiResult<CategoryViewModel>> GetById(int categoryId, int languageId);
        Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId,int pageIndex=0,int pageSize=0);
    }
}
