﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Blog
{
    public class BlogViewModel
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public int LikeCount { get; set; }
        public string ImagePath { get; set; }
        public DateTime Created_At { set; get; }
        public string UserName { set; get; }
        public string CategoryName { set; get; }
        public int CountComment { set; get; }
        public int Id { set; get; }
    }
}
