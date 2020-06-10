using eShopSolution.Data.Entities;
using System.Collections.Generic;

namespace eShopSolution.ViewModel.Catalog.Carts
{
    public class CartUpdateRequest
    {
        public int Id { set; get; }
        public List<CartProduct> CartProducts { set; get; }
    }
}
