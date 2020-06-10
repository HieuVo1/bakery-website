using System;

namespace eShopSolution.ViewModel.Catalog.Promotions
{
    public class PromotionViewModel
    {
        public int Id { set; get; }
        public string Code { set; get; }
        public int DiscountPercent { set; get; }
        public int DiscountAmount { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
    }
}
