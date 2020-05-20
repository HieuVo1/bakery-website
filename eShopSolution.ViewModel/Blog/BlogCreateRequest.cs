﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Blog
{
    public class BlogCreateRequest
    {
        public IFormFile ThumbnailImage { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Title { get; set; }
        public int LikeCount { get; set; }
        public Guid UserId { set; get; }
        [Required]
        public int CategoryId { set; get; }
    }
}
