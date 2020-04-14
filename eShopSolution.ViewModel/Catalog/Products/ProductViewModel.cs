using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class ProductViewModel
    {
        public int Id { set; get; }
        public int CategoryId { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public DateTime Created_At { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string ProductUrl { set; get; }
        public int LanguageId { set; get; }
    }
}
