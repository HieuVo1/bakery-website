using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Title{ get; set; }
        public int LikeCount{ get; set; }
        public string ImagePath { get; set; }
        public DateTime Created_At { set; get; }
        public Guid UserId { set; get; }
        public int CategoryId { set; get; }
        public UserApp UserApp { set; get; }
        public Category Categories { set; get; }
    }
}
