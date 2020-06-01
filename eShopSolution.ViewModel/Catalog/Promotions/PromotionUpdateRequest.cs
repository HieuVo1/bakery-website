using eShopSolution.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Promotions
{
    public class PromotionUpdateRequest
    {
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
