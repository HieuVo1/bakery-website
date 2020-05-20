using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        [Required]
        public IFormFile ThumbnailImage { get; set; }
    }
}
