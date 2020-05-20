using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Carts.CartItems
{
    public class CartItemUpdateRequest
    {
        public int ProductID { set; get; }
        public int CartID { set; get; }
        public int Quantity { set; get; }
    }
}
