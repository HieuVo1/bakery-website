using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Carts
{
    public class CartViewModel
    {
        public int Id { set; get; }
        public decimal TotalPrice { set; get; }
        public DateTime Created_At { set; get; }
        public Guid UserId { set; get; }
        public List<CartItemViewModel> CartItems { set; get; }
    }
}
