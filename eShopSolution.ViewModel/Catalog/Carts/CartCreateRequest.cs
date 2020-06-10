using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using System;
using System.Collections.Generic;

namespace eShopSolution.ViewModel.Catalog.Carts
{
    public class CartCreateRequest
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public DateTime Created_At { set; get; }
        public Guid UserId { set; get; }
        public List<CartItemViewModel> Items { set; get; }
    }
}
