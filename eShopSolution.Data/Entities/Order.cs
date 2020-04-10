using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Order
    {
        public int Id { set; get; }
        public int UserId { set; get; }
        public int CartId { set; get; }
        public int PromotionId { set; get; }
        public string ShipName { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhone { set; get; }
        public string ShipAddress { set; get; }
        public decimal Total { set; get; }
        public OrderStatus Status { set; get; }
        public DateTime Created_At { set; get; }
        public Promotion Promotion { set; get; }
        public List<OrderDetail> OrderDetails { set; get; }


    }
}
