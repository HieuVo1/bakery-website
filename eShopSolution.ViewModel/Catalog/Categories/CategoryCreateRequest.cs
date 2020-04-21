using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        public bool IsShowOnHome { set; get; }
        public CategoryStatus Status { set; get; }
        public string LanguageId { set; get; }
        public string Name { set; get; }
        public string CategoryUrl { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
