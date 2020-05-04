using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Categories
{
    public class CategoryUpdateRequest
    {
        [Required]
        public bool IsShowOnHome { set; get; }
        public CategoryStatus Status { set; get; }
        [Required]
        public string Name { set; get; }
        public string Description { set; get; }
        public string CategoryUrl { set; get; }
        public string LanguageId { set; get; }
        [Required]
        public IFormFile ThumbnailImage { get; set; }
    }
}
