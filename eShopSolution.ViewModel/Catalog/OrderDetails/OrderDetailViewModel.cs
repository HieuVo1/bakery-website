using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.OrderDetails
{
    public class OrderDetailViewModel
    {
        public int OrderId { set; get; }
        public string ProductName { set; get; }
        public decimal Price { set; get; }
        public string ImagePath { set; get; }
        public int Quantity { set; get; }
    }
}
