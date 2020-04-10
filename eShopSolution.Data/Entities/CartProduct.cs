using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class CartProduct
    {
        public int ProductID { set; get; }
        public Product Product { set; get; }
        public int CartID { set; get; }
        public Cart Cart { set; get; }
        public int Quantity { set; get; }
    }
}
