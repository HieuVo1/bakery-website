using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Carts
{
    public class CartUpdateRequest
    {
        public int Id { set; get; }
        public List<CartProduct> CartProducts { set; get; }
    }
}
