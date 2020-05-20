using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
