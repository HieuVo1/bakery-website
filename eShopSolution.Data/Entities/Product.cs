using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public string ProductUrl { set; get; }
        public DateTime Created_At { set; get; }

    }
}
