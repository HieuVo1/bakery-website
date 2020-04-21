using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Categories
{
    public class CategoryUpdateRequest
    {
        public bool IsShowOnHome { set; get; }
        public CategoryStatus Status { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string CategoryUrl { set; get; }
        public string LanguageId { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
