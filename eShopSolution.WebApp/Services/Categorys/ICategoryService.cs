
using eShopSolution.ViewModel.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Categorys
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetById(int categoryId, int languageId);
        Task<List<CategoryViewModel>> GetAll(string languageId,int pageIndex,int pageSize);
    }
}
