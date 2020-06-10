using eShopSolution.Data.Enums;
using System;

namespace eShopSolution.ViewModel.Catalog.Orders
{
    public class OrderViewModel
    {
        public int Id { set; get; }
        public string UserId { set; get; }
        public string UserName { set; get; }
        public int CartId { set; get; }
        public int PromotionId { set; get; }
        public int PromotionDiscount { set; get; }
        public string ShipName { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhone { set; get; }
        public string ShipAddress { set; get; }
        public string OrderNotes { set; get; }
        public decimal Total { set; get; }
        public OrderStatus Status { set; get; }
        public DateTime Created_At { set; get; }
    }
}
