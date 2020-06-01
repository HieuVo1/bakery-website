using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.OrderDetails
{
    public class OrderDetailUpdateRequest
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public decimal Price { set; get; }
        public int Quantity { set; get; }
    }
}
