using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Promotion
    {
        public int Id { set; get; }
        public string Code { set; get; }
        public int DiscountPercent { set; get; }
        public int DiscountAmount { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
        public Order Order { set; get; }
    }
}
