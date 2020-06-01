using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Like
    {
        public Guid UserId { set; get; }
        public int BlogId{ set; get; }
        public int Id { set; get; }
        public UserApp UserApp { set; get; }
    }
}
