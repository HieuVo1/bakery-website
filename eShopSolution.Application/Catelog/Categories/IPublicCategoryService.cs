using eShopSolution.ViewModel.Catalog.Categories;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Categories
{
    public interface IPublicCategoryService
    {
        public Task<List<CategoryViewModel>> GetAll(GetCategoryPaggingReqest request,string languageId);
    }
}
