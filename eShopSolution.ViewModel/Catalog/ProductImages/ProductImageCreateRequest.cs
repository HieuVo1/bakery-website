using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        [Required]
        public string Caption { get; set; }
        [Required]
        public int ProductId { set; get; }
        [Required]
        public bool IsDefault { get; set; }
       
        public IFormFile ThumbnailImage { get; set; }
    }
}
