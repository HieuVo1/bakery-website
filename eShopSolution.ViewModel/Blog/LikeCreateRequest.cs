using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Blog
{
    public class LikeCreateRequest
    {
        [Required]
        public int BlogId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
