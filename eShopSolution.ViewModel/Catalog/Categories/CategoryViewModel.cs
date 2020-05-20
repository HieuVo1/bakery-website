using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Categories
{
    public class CategoryViewModel
    {
        public int Id { set; get; }
        public bool IsShowOnHome { set; get; }
        public CategoryStatus Status { set; get; }
        public string LanguageId { set; get; }
        public string Language { set; get; }
        public string Name { set; get; }
        public string CategoryUrl { set; get; }
        public DateTime Created_At { set; get; }
        public string ImagePath { get; set; }
    }
}
