using eShopSolution.ViewModel.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Carts.CartItems
{
    public class CartItemViewModel
    {
        public ProductViewModel Product { get; set; }
        public int CartID { set; get; }
        public int Quantity { set; get; }
    }
}
