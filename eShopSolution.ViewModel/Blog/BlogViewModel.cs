using System;
using System.Collections.Generic;

namespace eShopSolution.ViewModel.Blog
{
    public class BlogViewModel
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public int LikeCount { get; set; }
        public bool Liked { set; get; }
        public string ImagePath { get; set; }
        public DateTime Created_At { set; get; }
        public string UserName { set; get; }
        public IEnumerable<string> UserLikes { set; get; }
        public string UserLikeId { set; get; }
        public string CategoryName { set; get; }
        public string CategoryUrl { set; get; }
        public int CountComment { set; get; }
        public int Id { set; get; }
    }
}
