using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductImageUpdateRequest
    {
        public string ImagePath { get; set; }

        public string Caption { get; set; }
        public long FileSize { set; get; }

        public bool IsDefault { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
