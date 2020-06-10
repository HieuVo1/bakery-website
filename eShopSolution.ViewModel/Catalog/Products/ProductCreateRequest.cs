using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { set; get; }
        [Required]
        public decimal OriginalPrice { set; get; }
        [Required]
        public int Stock { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string Description { set; get; }
        public string ProductUrl { set; get; }
        public string LanguageId { set; get; }
        [Required]
        public IFormFile ThumbnailImage { get; set; }
    }
}
