using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string Description { set; get; }
        public string ProductUrl { set; get; }
        public string LanguageId { set; get; }
        [Required]
        public int CategoryId { set; get; }
        [Required]
        public int Stock { set; get; }
        [Required]
        public decimal Price { set; get; }
        [Required]
        public decimal OriginalPrice { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
