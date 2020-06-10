using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        [Required]
        public string Caption { get; set; }
        [Required]
        public bool IsDefault { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
