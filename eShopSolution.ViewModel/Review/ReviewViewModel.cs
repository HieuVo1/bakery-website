using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Review
{
    public class ReviewViewModel
    {
        public int ProductId { get; set; }
        public int Id { get; set; }
        public DateTime Created_At { set; get; }
        public string Content { set; get; }
        public string Email { set; get; }
        public string Name { set; get; }
        public string ImagePath { set; get; }
        public int Rating { set; get; }
    }
}
