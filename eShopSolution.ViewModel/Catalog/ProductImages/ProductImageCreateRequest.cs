using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        public string Caption { get; set; }
        public int productId { set; get; }
        public bool IsDefault { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
