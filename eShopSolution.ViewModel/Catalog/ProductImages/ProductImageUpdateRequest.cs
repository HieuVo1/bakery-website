using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {

        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
