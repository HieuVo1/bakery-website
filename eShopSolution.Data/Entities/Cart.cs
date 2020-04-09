using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Cart
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public DateTime Created_At { set; get; }
    }
}
