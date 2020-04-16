using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ImagePath { get; set; }

        public string Caption { get; set; }
        public long FileSize { set; get; }

        public bool IsDefault { get; set; }

        public Product Product { get; set; }
    }
}
