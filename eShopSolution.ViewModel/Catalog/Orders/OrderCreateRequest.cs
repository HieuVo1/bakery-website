using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Catalog.OrderDetails;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Orders
{
    public class OrderCreateRequest
    {
        public string UserId { set; get; }
        public int PromotionId { set; get; }
        public string ShipName { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhone { set; get; }
        public string ShipAddress { set; get; }
        public string Street { set; get; }
        public string OrderNotes { set; get; }
        public decimal Total { set; get; }
        public OrderStatus Status { set; get; }
        public DateTime Created_At { set; get; }
        public List<OrderDetailCreateRequest> OrderDetails { set; get; }
    }
}
