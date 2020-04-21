using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string ProductUrl { set; get; }
        public string LanguageId { set; get; }
        public int CategoryId { set; get; }
        public int Stock { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
