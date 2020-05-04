using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        [Required]
        public string Caption { get; set; }
        [Required]
        public bool IsDefault { get; set; }
        [Required]
        public IFormFile ThumbnailImage { get; set; }
    }
}
