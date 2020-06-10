using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Review
{
    public class ReviewUpdateRequest
    {
        [Required]
        public string Content { set; get; }
    }
}
