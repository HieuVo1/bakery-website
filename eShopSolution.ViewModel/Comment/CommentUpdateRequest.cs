using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Comment
{
    public class CommentUpdateRequest
    {
        public int BlogId { get; set; }
        [Required]
        public string Content { set; get; }

    }
}
