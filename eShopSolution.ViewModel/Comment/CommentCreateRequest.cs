using System;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Comment
{
    public class CommentCreateRequest
    {
        public Guid UserId { get; set; }
        public int BlogId { get; set; }
        [Required]
        public string Content { set; get; }
    }
}
