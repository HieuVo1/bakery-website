using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Comment
{
    public class CommentUpdateRequest
    {
        public int BlogId { get; set; }
        [Required]
        public string Content { set; get; }

    }
}
