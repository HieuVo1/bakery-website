using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public int CategoryId { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public DateTime Created_At { set; get; }
        public OrderDetail OrderDetail { set; get; }
        public Category Category { set; get; }
        public List<CartProduct> CartProducts { set; get; }
        public List<ProductImage> ProductImages { set; get; }
        public List<ProductTranslation> ProductTranslations { set; get; }

    }
}
