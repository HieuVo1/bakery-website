using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Comment
    {
        public Guid UserId { get; set; }
        public int BlogId { get; set; }
        public int Id { get; set; }
        public DateTime Created_At { set; get; }
        public string Content{ set; get; }
        public UserApp UserApp { set; get; }
    }
}
