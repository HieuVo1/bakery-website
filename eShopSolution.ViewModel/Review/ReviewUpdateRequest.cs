using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Review
{
    public class ReviewUpdateRequest
    {
        [Required]
        public string Content { set; get; }
    }
}
