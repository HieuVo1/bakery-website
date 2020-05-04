using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        [Required(ErrorMessage ="IsShowHome is required")]
        public bool IsShowOnHome { set; get; }
        public CategoryStatus Status { set; get; }
        public string LanguageId { set; get; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { set; get; }
        public string CategoryUrl { set; get; }
        [Required(ErrorMessage = "ThumbnailImage is required")]
        public IFormFile ThumbnailImage { get; set; }
    }
}
