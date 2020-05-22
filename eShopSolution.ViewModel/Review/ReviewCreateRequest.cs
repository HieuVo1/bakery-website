using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Review
{
    public class ReviewCreateRequest
    {

        [Required]
        public string Content { set; get; }
        [Required]
        public string Email { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public int Rating { set; get; }
        [Required]
        public int ProductId { set; get; }
    }
}
