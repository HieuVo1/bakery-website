using eShopSolution.ViewModel.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Catalog.Promotions
{
    public class PromotionCreateRequest
    {
        [Required]
        public string Code { set; get; }
        public int DiscountPercent { set; get; }
        [Required]
        public int DiscountAmount { set; get; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { set; get; }
        [Required]
        [DataType(DataType.Date)]
        [DateMoreThan("FromDate", ErrorMessage = "ToDate is more than FromDate")]
        public DateTime ToDate { set; get; }
    }
}
