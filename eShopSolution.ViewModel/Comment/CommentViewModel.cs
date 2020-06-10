using System;

namespace eShopSolution.ViewModel.Comment
{
    public class CommentViewModel
    {
        public int Id { set; get; }
        public DateTime Created_At { set; get; }
        public string Content { set; get; }
        public string  UserName { set; get; }
        public string  ImagePath { set; get; }
        public string  time { set; get; }
    }
}
