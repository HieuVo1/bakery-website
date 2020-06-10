using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
        public IFormFile ThumbnailImage { get; set; }
    }
}
